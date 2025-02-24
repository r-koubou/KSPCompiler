using System.Collections.Generic;

using KSPCompiler.Commons.Text;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Statements;

namespace KSPCompiler.Applications.LSPServer.Omnisharp.Ast;

public sealed class VariableSymbolAppearanceFinder(
    string symbolName,
    AppearanceFinderMode mode = AppearanceFinderMode.All )
    : DefaultAstVisitor, IAstAppearanceFinder
{
    private string SymbolName { get; } = symbolName;
    private AppearanceFinderMode Mode { get; } = mode;
    private List<Position> Result { get; } = [];

    public IReadOnlyCollection<Position> Find( AstCompilationUnitNode ast )
    {
        Result.Clear();

        ast.AcceptChildren( this );

        return new List<Position>( Result );
    }

    public override IAstNode Visit( AstVariableDeclarationNode node )
    {
        if( Mode.HasFlag( AppearanceFinderMode.Declaration ) && node.Name == SymbolName )
        {
            Result.Add( node.VariableNamePosition );
        }

        return base.Visit( node );
    }

    public override IAstNode Visit( AstSymbolExpressionNode node )
    {
        if( Mode.HasFlag( AppearanceFinderMode.Reference ) && node.Name == SymbolName )
        {
            Result.Add( node.Position );
        }

        return base.Visit( node );
    }
}
