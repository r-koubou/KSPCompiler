using System.Collections.Generic;

using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Blocks;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Statements;
using KSPCompiler.Shared.Text;

namespace KSPCompiler.Features.LanguageServer.UseCase.Ast;

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
