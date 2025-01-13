using System.Collections.Generic;

using KSPCompiler.Commons.Text;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.LSPServer.Core.Ast;

public sealed class UserFunctionSymbolAppearanceFinder( string symbolName, ISymbolTable<UserFunctionSymbol> symbolTable )
    : DefaultAstVisitor, ISymbolAppearanceFinder
{
    private string SymbolName { get; } = symbolName;
    private ISymbolTable<UserFunctionSymbol> SymbolTable { get; } = symbolTable;
    private List<Position> Result { get; } = [];

    public IReadOnlyCollection<Position> Find( AstCompilationUnitNode ast )
    {
        Result.Clear();

        ast.AcceptChildren( this );

        return new List<Position>( Result );
    }

    public override IAstNode Visit( AstUserFunctionDeclarationNode node )
    {
        if( node.Name == SymbolName )
        {
            Result.Add( node.FunctionNamePosition );
        }

        return base.Visit( node );
    }

    public override IAstNode Visit( AstCallUserFunctionStatementNode node )
    {
        if( node.Symbol.Name == SymbolName )
        {
            Result.Add( node.Symbol.Position );
        }

        return base.Visit( node );
    }
}
