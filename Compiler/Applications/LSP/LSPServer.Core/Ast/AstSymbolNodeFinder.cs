using KSPCompiler.Applications.LSPServer.Core.Extensions;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Statements;

using OmniSharpPosition = OmniSharp.Extensions.LanguageServer.Protocol.Models.Position;

namespace KSPCompiler.Applications.LSPServer.Core.Ast;



public sealed class AstSymbolNodeFinder : DefaultAstVisitor
{
    private OmniSharpPosition Position { get; set; } = new();
    private IAstNode? Result { get; set; }

    public bool TryFindNode( AstCompilationUnitNode ast, OmniSharpPosition position, out IAstNode result )
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

    private static bool IsPositionInNode( OmniSharpPosition position, IAstNode node )
    {
        var range = node.Position.AsRange();

        return range.Start.Line == position.Line
               && position.Character >= range.Start.Character
               && position.Character <= range.End.Character;
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
