using KSPCompiler.Domain.Ast.Node;
using KSPCompiler.Domain.Ast.Node.Blocks;
using KSPCompiler.Domain.Ast.Node.Statements;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.CompilerMessages.Extensions;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.Extensions;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;

using Resource = KSPCompiler.Resources.CompilerMessageResources;

namespace KSPCompiler.Domain.Ast.Analyzers;

#error TODO ビルトインなど予約済みのシンボルを事前にファイルからロードする（変数、コールバック、コマンド）
// 外部で事前にロードした結果をコンストラクタで受け取る(ISymbolTable<T>で)

public class SymbolCollector : DefaultAstVisitor, ISymbolCollector
{
    private ICompilerMessageManger CompilerMessageManger { get; }

    #region Symbol Tables
    public ISymbolTable<VariableSymbol> Variables { get; } = new VariableSymbolTable();
    public ISymbolTable<UITypeSymbol> UITypes { get; } = new UITypeSymbolTable();
    public ISymbolTable<CallbackSymbol> Callbacks { get; } = new CallbackSymbolTable();
    public ISymbolTable<CallbackSymbol> ReservedCallbacks { get; } = new CallbackSymbolTable();
    #endregion

    public SymbolCollector( ICompilerMessageManger compilerMessageManger )
    {
        CompilerMessageManger = compilerMessageManger;
    }

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
            CompilerMessageManger.Error( Resource.symbol_error_declare_variable_ni_reserved, name );
        }
        #endregion

        //--------------------------------------------------------------------------
        #region on init 外での変数宣言はエラー
        //--------------------------------------------------------------------------
        if( !node.TryGetParent<AstCallbackDeclaration>( out var callback ) )
        {
            CompilerMessageManger.Fatal( Resource.syntax_error );
        }
        else
        {
            if( callback.Name != "init" )
            {
                CompilerMessageManger.Error( Resource.symbol_error_declare_variable_outside, name );
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
                return true;
            }
            // NI の予約変数との重複
            if( variable.Reserved )
            {
                CompilerMessageManger.Error( Resource.symbol_error_declare_variable_reserved, name );
                return false;
            }

            // ユーザー変数との重複
            CompilerMessageManger.Error( Resource.symbol_error_declare_variable_already, name );
            return false;
        }
        #endregion
    }

    private void ValidateVariable( AstVariableDeclaration node, VariableSymbol variable )
    {
        variable.DataTypeModifier |= ( node.Modifier ) switch
        {
            "const"      => DataTypeModifierFlag.Const,
            "polyphonic" => DataTypeModifierFlag.Polyphonic,
            // キーワード以外の場合は暫定で UI として、以降で判定
            _            => DataTypeModifierFlag.UI
        };

        #region UI変数チェック
        // 非プリミティブ型 (UI)
        if( variable.DataTypeModifier.IsUI() )
        {
            // 有効な UI 型かチェック
            if( !UITypes.TrySearchByName( node.Modifier, out var uiType ) )
            {
                CompilerMessageManger.Warning( Resource.symbol_error_declare_variable_unkown, node.Modifier );

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
            variable.DataType = DataTypeUtility.Guess( variable.Name );
        }
        #endregion
    }

    #endregion ~Variable Collection

    #region Callback Collection

    public override IAstNode Visit( AstCallbackDeclaration node )
    {
        if( node.ArgumentList.HasArgument )
        {
            // コールバック引数リストあり
            // 現状、コールバック引数は　on init で宣言した変数
            foreach( var arg in node.ArgumentList.Arguments )
            {
                if( !Variables.TrySearchByName( arg.Name, out var variable ) )
                {
                    // on init で未定義
                    CompilerMessageManger.Error( Resource.symbol_error_declare_variable_unkown, arg.Name );
                }
                else
                {
                    variable.State = VariableState.Loaded;
                }
            }
        }

        #error TODO 予約済みコールバックの検査

        return node;
    }

    #endregion

}
