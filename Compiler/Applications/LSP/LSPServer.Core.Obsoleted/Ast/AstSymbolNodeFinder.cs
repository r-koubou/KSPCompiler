using KSPCompiler.Commons.Text;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Statements;

namespace KSPCompiler.Applications.LSPServer.Core.Ast;

public sealed class AstSymbolNodeFinder : DefaultAstVisitor
{
    private Position Position { get; set; }
    private IAstNode? Result { get; set; }

    public bool TryFindNode( AstCompilationUnitNode ast, Position position, out IAstNode result )
    {
        Result   = null;
        result   = null!;
        Position = position;

        ast.AcceptChildren( this );

        if( Result == null )
        {
            return false;
        }

        result = Result;

        return true;
    }

    private static bool IsPositionInNode( Position position, IAstNode node )
    {
        var range = node.Position;

        return range.BeginLine.Value == position.BeginLine.Value
               && position.BeginColumn.Value >= range.BeginColumn.Value
               && position.EndColumn.Value <= range.EndColumn.Value;
    }

    public override IAstNode Visit( AstCallbackDeclarationNode node )
    {
        if( IsPositionInNode( Position, node ) )
        {
            Result = node;
        }

        return base.Visit( node );
    }

    public override IAstNode Visit( AstUserFunctionDeclarationNode node )
    {
        if( IsPositionInNode( Position, node ) )
        {
            Result = node;
        }

        return base.Visit( node );
    }

    public override IAstNode Visit( AstVariableDeclarationNode node )
    {
        if( IsPositionInNode( Position, node ) )
        {
            Result = node;
        }

        return base.Visit( node );
    }

    public override IAstNode Visit( AstSymbolExpressionNode node )
    {
        if( IsPositionInNode( Position, node ) )
        {
            Result = node;
        }

        return base.Visit( node );
    }
}
