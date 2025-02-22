using System.Collections.Generic;

using KSPCompiler.Commons.Text;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Ast.Nodes.Statements;

namespace KSPCompiler.Applications.LSPServer.CoreNew.Ast;

public sealed class FoldingSupportedNodeAppearanceFinder
    : DefaultAstVisitor, IAstAppearanceFinder
{
    private List<Position> Result { get; } = [];

    public IReadOnlyCollection<Position> Find( AstCompilationUnitNode ast )
    {
        Result.Clear();

        ast.AcceptChildren( this );

        return new List<Position>( Result );
    }

    public override IAstNode Visit( AstPreprocessorIfdefineNode node )
    {
        Result.Add( node.Position );
        return base.Visit( node );
    }

    public override IAstNode Visit( AstPreprocessorIfnotDefineNode node )
    {
        Result.Add( node.Position );
        return base.Visit( node );
    }

    public override IAstNode Visit( AstIfStatementNode node )
    {
        Result.Add( node.Position );
        return base.Visit( node );
    }

    public override IAstNode Visit( AstWhileStatementNode node )
    {
        Result.Add( node.Position );
        return base.Visit( node );
    }

    public override IAstNode Visit( AstSelectStatementNode node )
    {
        Result.Add( node.Position );
        return base.Visit( node );
    }
}
