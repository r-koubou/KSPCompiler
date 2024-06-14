using KSPCompiler.Domain.Ast.Node.Statements;

namespace KSPCompiler.Domain.Ast.Node.Extensions;

public static class AstNodeExtension
{
    /// <summary>
    /// Check if the node is Null Object
    /// </summary>
    /// <param name="node">A node instance</param>
    /// <returns>true if the node is Null Object otherwise false.</returns>
    /// <seealso cref="NullAstNode"/>
    /// <seealso cref="NullAstExpressionSyntaxNode"/>
    /// <seealso cref="NullAstExpressionSyntaxNode"/>
    public static bool IsNull( this IAstNode? node )
        => node is null
            or NullAstNode
            or NullAstExpressionSyntaxNode
            or NullAstInitializer;

    /// <summary>
    /// Check if the node is not Null Object
    /// </summary>
    /// <param name="node">A node instance</param>
    /// <returns>true if the node is Null Object otherwise false.</returns>
    /// <seealso cref="NullAstNode"/>
    /// <seealso cref="NullAstExpressionSyntaxNode"/>
    /// <seealso cref="NullAstExpressionSyntaxNode"/>
    public static bool IsNotNull( this IAstNode? node )
        => !node.IsNull();
}
