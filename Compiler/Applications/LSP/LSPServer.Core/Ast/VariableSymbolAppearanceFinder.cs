using System.Collections.Generic;

using KSPCompiler.Commons.Text;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.LSPServer.Core.Ast;

public sealed class VariableSymbolAppearanceFinder(
    string symbolName,
    bool referenceOnly = false )
    : DefaultAstVisitor, ISymbolAppearanceFinder
{
    private string SymbolName { get; } = symbolName;
    private bool ReferenceOnly { get; } = referenceOnly;
    private List<Position> Result { get; } = [];

    public IReadOnlyCollection<Position> Find( AstCompilationUnitNode ast )
    {
        Result.Clear();

        ast.AcceptChildren( this );

        return new List<Position>( Result );
    }

    public override IAstNode Visit( AstVariableDeclarationNode node )
    {
        if( !ReferenceOnly && node.Name == SymbolName )
        {
            Result.Add( node.VariableNamePosition );
        }

        return base.Visit( node );
    }

    public override IAstNode Visit( AstSymbolExpressionNode node )
    {
        if( node.Name == SymbolName )
        {
            Result.Add( node.Position );
        }

        return base.Visit( node );
    }
}
