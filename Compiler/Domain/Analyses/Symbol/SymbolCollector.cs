using KSPCompiler.Commons;
using KSPCompiler.Domain.Ast.Node;
using KSPCompiler.Domain.Ast.Node.Blocks;
using KSPCompiler.Domain.Ast.Node.Statements;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;

namespace KSPCompiler.Domain.Analyses.Symbol;

public class SymbolCollector : AstVisitorAdaptor<Unit>, ISymbolCollector
{
    public SymbolTable<VariableSymbol> VariableTable { get; } = new VariableSymbolTable();
    public SymbolTable<UserFunctionSymbol> UserFunctionTable { get; } = new UserFunctionSymbolTable();
    public ISymbolFactory<AstVariableDeclaration, VariableSymbol> VariableSymbolFactory { get; }
    public ISymbolFactory<AstUserFunctionDeclaration, UserFunctionSymbol> UserFunctionSymbolFactory { get; }

    private readonly ICompilerMessageManger compilerMessageManager;

    public SymbolCollector(
        ICompilerMessageManger compilerMessageManager,
        ISymbolFactory<AstVariableDeclaration, VariableSymbol> variableSymbolFactory,
        ISymbolFactory<AstUserFunctionDeclaration, UserFunctionSymbol> userFunctionSymbolFactory )
    {
        this.compilerMessageManager = compilerMessageManager;
        VariableSymbolFactory       = variableSymbolFactory;
        UserFunctionSymbolFactory   = userFunctionSymbolFactory;
    }

    public void Collect( AstCompilationUnit root )
    {
        _ = root.Accept( this );
        throw new System.NotImplementedException();
    }

    #region Variables
    public override Unit Visit( AstVariableDeclaration node )
    {
        // 変数は on init コールバックブロック内でしか宣言できない
        if( !TryGetDeclaredBlock( node, out var currentCallback ) )
        {
            // TODO i18n
            var message = compilerMessageManager.MessageFactory.Create( CompilerMessageLevel.Error, "declaration outside of callback scope" );
            compilerMessageManager.Append( message );
        }

        // TryGetDeclaredBlockで値はセットされるので、通常ここには到達しないはず
        if( currentCallback == null )
        {
            throw new SymbolCollectorException( "declaration callback block not found" );
        }

        // 宣言場所のチェック
        ValidateDeclaredInOnInitCallback( currentCallback );

        // NI が予約している接頭文字が含んでいないかチェック
        if( !KspValueConstants.ContainsNiReservedPrefix( node.Name ) )
        {
            // TODO i18n
            var message = compilerMessageManager.MessageFactory.Create( CompilerMessageLevel.Warning, "variable name contains NI reserved prefix" );
            compilerMessageManager.Append( message );
        }

        // テーブル内の変数名と重複していないかチェック
        if( VariableTable.TrySearchByName( node.Name, out var symbol ) )
        {
            // ビルトイン変数との重複
            if( symbol.IsReserved )
            {
                // TODO i18n
                var message = compilerMessageManager.MessageFactory.Create( CompilerMessageLevel.Error, $"variable name is reserved: {node.Name}" );
                compilerMessageManager.Append( message );
            }
            // ユーザー定義変数との重複
            else
            {
                // TODO i18n
                var message = compilerMessageManager.MessageFactory.Create( CompilerMessageLevel.Error, $"variable name is already defined: {node.Name}" );
                compilerMessageManager.Append( message );
            }

            return Unit.Default;
        }

        var newSymbol = VariableSymbolFactory.Create( node );

        #error TODO UI変数チェック / 外部定義とのマージ
        if( newSymbol.DataModifier.IsUI() )
        {
        }

        //テーブルに登録
        VariableTable.Add( node.Name, newSymbol );

        return Unit.Default;
    }
    #endregion ~ Variables

    // TODO
    #region User Functions
    public override Unit Visit( AstUserFunctionDeclaration node )
    {
        return Unit.Default;
    }
    #endregion ~ User Functions

    #region Common Processes

    private static bool TryGetDeclaredBlock( IAstNode node, out AstCallbackDeclaration? currentCallback )
    {
        currentCallback = null;

        var parent = node.Parent;
        do
        {
            if( node.Parent is not AstCallbackDeclaration callbackDeclaration )
            {
                continue;
            }

            currentCallback = callbackDeclaration;
            break;
        }while( ( parent = parent?.Parent ) != null );

        return currentCallback != null;
    }

    // TODO 意味解析フェーズで判定させる
    private void ValidateDeclaredInOnInitCallback( AstCallbackDeclaration callbackDeclaration )
    {
        if( callbackDeclaration.Name == "on init" )
        {
            return;
        }

        // TODO i18n
        var message = compilerMessageManager.MessageFactory.Create( CompilerMessageLevel.Warning, "declaration outside of on init callback" );
        compilerMessageManager.Append( message );
    }

    #endregion
}
