using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Interactors.Analysis.Semantics.Extensions;

public static class AstLiteralNodeExtension
{
    public static bool IsLiteralNode( this IAstNode self )
        => self is
            AstIntLiteralNode or
            AstRealLiteralNode or
            AstStringLiteralNode or
            AstBooleanLiteralNode;
}
