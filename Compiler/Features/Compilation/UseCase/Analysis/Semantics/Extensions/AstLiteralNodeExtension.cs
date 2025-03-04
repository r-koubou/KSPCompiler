using KSPCompiler.Shared.Domain.Ast.Nodes;
using KSPCompiler.Shared.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Semantics.Extensions;

public static class AstLiteralNodeExtension
{
    public static bool IsLiteralNode( this IAstNode self )
        => self is
            AstIntLiteralNode or
            AstRealLiteralNode or
            AstStringLiteralNode or
            AstBooleanLiteralNode;

    public static bool TryGetLiteralNodeValue( this IAstNode self, out object? value )
    {
        value = null;

        if( self is AstIntLiteralNode intNode )
        {
            value = intNode.Value;
            return true;
        }

        if( self is AstRealLiteralNode realNode )
        {
            value = realNode.Value;
            return true;
        }

        if( self is AstStringLiteralNode stringNode )
        {
            value = stringNode.Value;
            return true;
        }

        if( self is AstBooleanLiteralNode boolNode )
        {
            value = boolNode.Value;
            return true;
        }

        return false;
    }
}
