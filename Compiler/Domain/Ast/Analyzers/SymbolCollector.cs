using KSPCompiler.Domain.Ast.Extensions;
using KSPCompiler.Domain.Ast.Node;
using KSPCompiler.Domain.Ast.Node.Blocks;
using KSPCompiler.Domain.Ast.Node.Statements;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.Extensions;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;

using Resource = KSPCompiler.Resources.CompilerMessageResources;

namespace KSPCompiler.Domain.Ast.Analyzers;

// TODO ビルトインなど予約済みのシンボルを事前にファイルからロードする（変数、コールバック、コマンド）
// 外部で事前にロードした結果をコンストラクタで受け取る(ISymbolTable<T>で)

public sealed class SymbolCollector : DefaultAstVisitor, ISymbolCollector
{
    private ICompilerMessageManger CompilerMessageManger { get; }

    #region Symbol Tables
    public ISymbolTable<VariableSymbol> Variables { get; } = new VariableSymbolTable();
    public ISymbolTable<UITypeSymbol> UITypes { get; } = new UITypeSymbolTable();
    public ISymbolTable<CallbackSymbol> Callbacks { get; } = new CallbackSymbolTable();
    public ISymbolTable<UserFunctionSymbol> UserFunctions { get; } = new UserFunctionSymbolTable();


    private ISymbolTable<VariableSymbol> ReservedVariables { get; }
    private ISymbolTable<UITypeSymbol> ReservedUITypes { get; }
    private ISymbolTable<CallbackSymbol> ReservedCallbacks { get; }
    private ISymbolTable<CommandSymbol> ReservedCommands { get; }

    #endregion

    public SymbolCollector(
        ICompilerMessageManger compilerMessageManger,
        ISymbolTable<VariableSymbol> reservedVariables,
        ISymbolTable<UITypeSymbol> reservedUITypes,
        ISymbolTable<CallbackSymbol> reservedCallbacks,
        ISymbolTable<CommandSymbol> reservedCommands )
    {
        CompilerMessageManger = compilerMessageManger;
        ReservedVariables     = reservedVariables;
        ReservedUITypes       = reservedUITypes;
        ReservedCallbacks     = reservedCallbacks;
        ReservedCommands      = reservedCommands;
    }

    public SymbolCollector( ICompilerMessageManger compilerMessageManger ) : this(
        compilerMessageManger,
        new VariableSymbolTable(),
        new UITypeSymbolTable(),
        new CallbackSymbolTable(),
        new CommandSymbolTable()
    )
    {}

    public void Analyze( AstCompilationUnit node )
    {
        node.Accept( this );
    }

    #region Variable Collection
    public override IAstNode Visit( AstVariableDeclaration node )
    {
        if( !ValidateVariableDeclaration( node, out var variable ) || variable.IsNull() )
        {
            return node;
        }

        Variables.Add( variable );
        ValidateVariable( node, variable );

        return node;
    }

    private bool ValidateVariableDeclaration( AstVariableDeclaration node, out VariableSymbol variable )
    {
        var name = node.Name;
        variable = NullVariableSymbol.Instance;

        //--------------------------------------------------------------------------
        #region 予約済み（NIが禁止している）接頭語検査
        //--------------------------------------------------------------------------
        if( KspValueConstants.ContainsNiReservedPrefix( node.Name ) )
        {
            CompilerMessageManger.Error( node, Resource.symbol_error_declare_variable_ni_reserved, name );
        }
        #endregion

        //--------------------------------------------------------------------------
        #region on init 外での変数宣言はエラー
        //--------------------------------------------------------------------------
        if( !node.TryGetParent<AstCallbackDeclaration>( out var callback ) )
        {
            CompilerMessageManger.Fatal( node, Resource.syntax_error );
        }
        else
        {
            if( callback.Name != "init" )
            {
                CompilerMessageManger.Error( node, Resource.symbol_error_declare_variable_outside, name );
                return false;
            }
        }
        #endregion

        //--------------------------------------------------------------------------
        #region 定義済みの検査
        //--------------------------------------------------------------------------
        {
            if( !Variables.TrySearchByName( name, out variable ) )
            {
                // 未定義：新規追加可能
                variable = node.As();
                return true;
            }
            // NI の予約変数との重複
            if( variable.Reserved )
            {
                CompilerMessageManger.Error( node, Resource.symbol_error_declare_variable_reserved, name );
                return false;
            }

            // ユーザー変数との重複
            CompilerMessageManger.Error( node, Resource.symbol_error_declare_variable_already, name );
            return false;
        }
        #endregion
    }

    private void ValidateVariable( AstVariableDeclaration node, VariableSymbol variable )
    {
        #region UI変数チェック
        // 非プリミティブ型 (UI)
        if( variable.DataTypeModifier.IsUI() )
        {
            // 有効な UI 型かチェック
            if( !UITypes.TrySearchByName( node.Modifier, out var uiType ) )
            {
                CompilerMessageManger.Warning( node, Resource.symbol_error_declare_variable_unkown, node.Modifier );

                return;
            }

            // そのUI型は後から変更不可能な仕様の場合
            if( uiType.DataTypeModifier.IsConstant() )
            {
                variable.DataTypeModifier |= DataTypeModifierFlag.Const;
            }

            // UI型情報を参照
            // 意味解析時に使用するため、用意されている変数に保持
            variable.UIType = uiType;
        }
        // プリミティブ型
        else
        {
            variable.DataType = DataTypeUtility.GuessFromSymbolName( variable.Name );
        }
        #endregion
    }

    #endregion ~Variable Collection

    #region Callback Collection

    public override IAstNode Visit( AstCallbackDeclaration node )
    {
        node.AcceptChildren( this );

        if( node.ArgumentList.HasArgument )
        {
            // コールバック引数リストあり
            // 現状、コールバック引数は　on init で宣言した変数
            foreach( var arg in node.ArgumentList.Arguments )
            {
                if( !Variables.TrySearchByName( arg.Name, out var variable ) )
                {
                    // on init で未定義
                    CompilerMessageManger.Error( node, Resource.symbol_error_declare_variable_unkown, arg.Name );
                }
                else
                {
                    variable.Referenced = true;
                }
            }
        }

        CallbackSymbol thisCallback;

        // NI予約済みコールバックの検査
        if( !ReservedCallbacks.TrySearchByName( node.Name, out var reservedCallback ) )
        {
            CompilerMessageManger.Warning( node, Resource.symbol_warning_declare_callback_unkown, node.Name );

            // 暫定のシンボル生成
            thisCallback = node.As();
        }
        else
        {
            thisCallback = reservedCallback;
        }

        if( !Callbacks.Add( thisCallback ) )
        {
            CompilerMessageManger.Error( node, Resource.symbol_error_declare_callback_already, node.Name );
        }

        return node;
    }

    #endregion ~Callback Collection

    #region User function Collection

    public override IAstNode Visit( AstUserFunctionDeclaration node )
    {
        node.AcceptChildren( this );

        var thisUserFunction = node.As();

        if( !UserFunctions.Add( thisUserFunction ) )
        {
            CompilerMessageManger.Error( node, Resource.symbol_error_declare_userfunction_already, node.Name );
        }

        return node;
    }

    #endregion ~User function Collection
}
