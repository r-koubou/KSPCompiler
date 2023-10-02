using System;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Ast.Node;
using KSPCompiler.Domain.Ast.Node.Blocks;
using KSPCompiler.Domain.Ast.Node.Statements;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;
using KSPCompiler.Resources;

namespace KSPCompiler.Domain.Analyses.Symbol;

public class VariableSymbolCollector : AstVisitorAdaptor<Unit>, ISymbolCollector<VariableSymbol>
{
    private ISymbolFactory<AstVariableDeclaration, VariableSymbol> VariableSymbolFactory { get; }
    private readonly ICompilerMessageManger compilerMessageManager;

    public ISymbolTable<VariableSymbol> SymbolTable { get; } = new VariableSymbolTable();

    public bool HasError { get; private set; }

    public VariableSymbolCollector(
        ICompilerMessageManger compilerMessageManager,
        ISymbolFactory<AstVariableDeclaration, VariableSymbol> variableSymbolFactory )
    {
        this.compilerMessageManager = compilerMessageManager;
        VariableSymbolFactory       = variableSymbolFactory;
    }

    public ISymbolTable<VariableSymbol> Collect( AstCompilationUnit root )
    {
        throw new NotImplementedException();
        HasError = false;
        root.Accept( this );
        return SymbolTable;
    }

    #region Variables
    public override Unit Visit( AstVariableDeclaration node )
    {
        // 変数は on init コールバックブロック内でしか宣言できない
        if( !node.TryGetParent<AstCallbackDeclaration>( out var currentCallback ) )
        {
            var text = string.Format( CompilerMessageResources.symbol_error_declare_variable_outside, node.Name);
            var message = compilerMessageManager.MessageFactory.Error( text );
            compilerMessageManager.Append( message );
            HasError = true;

            return Unit.Default;
        }

        // TryGetDeclaredBlockで値はセットされるので、通常ここには到達しないはず
        if( currentCallback == null )
        {
            throw new SymbolCollectorException( "declaration callback block not found" );
        }

        // 宣言場所のチェック
        ValidateDeclaredInOnInitCallback( currentCallback, node );

        // NI が予約している接頭文字が含んでいないかチェック
        if( !KspValueConstants.ContainsNiReservedPrefix( node.Name ) )
        {
            var text = string.Format( CompilerMessageResources.symbol_error_declare_variable_ni_reserved, node.Name);
            var message = compilerMessageManager.MessageFactory.Warning( text );
            compilerMessageManager.Append( message );
        }

        // テーブル内の変数名と重複していないかチェック
        if( SymbolTable.TrySearchByName( node.Name, out var symbol ) )
        {
            // ビルトイン変数との重複
            if( symbol.IsReserved )
            {
                var text = string.Format( CompilerMessageResources.symbol_error_declare_variable_reserved, node.Name);
                var message = compilerMessageManager.MessageFactory.Error( text );
                compilerMessageManager.Append( message );
                HasError = true;
            }
            // ユーザー定義変数との重複
            else
            {
                var text = string.Format( CompilerMessageResources.symbol_error_declare_variable_already, node.Name);
                var message = compilerMessageManager.MessageFactory.Error( text );
                compilerMessageManager.Append( message );
                HasError = true;
            }

            return Unit.Default;
        }

        var newSymbol = VariableSymbolFactory.Create( node );

        #error TODO UI変数チェック / 外部定義とのマージ
        if( newSymbol.DataModifier.IsUI() )
        {
        }

        //テーブルに登録
        SymbolTable.Add( node.Name, newSymbol );

        return Unit.Default;
    }
    #endregion ~ Variables

    #region Common Processes

    // TODO 意味解析フェーズで判定させる
    private void ValidateDeclaredInOnInitCallback( AstCallbackDeclaration callbackDeclaration, AstVariableDeclaration node )
    {
        if( callbackDeclaration.Name == "on init" )
        {
            return;
        }

        var text = string.Format( CompilerMessageResources.symbol_waring_declare_oninit, node.Name);
        var message = compilerMessageManager.MessageFactory.Warning( text );
        compilerMessageManager.Append( message );
    }

    #endregion
}
