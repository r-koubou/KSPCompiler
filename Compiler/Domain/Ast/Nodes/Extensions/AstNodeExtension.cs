using KSPCompiler.Domain.Ast.Nodes.Statements;

namespace KSPCompiler.Domain.Ast.Nodes.Extensions;

public static class AstNodeExtension
{
    /// <summary>
    /// Check if the node is Null Object
    /// </summary>
    /// <param name="node">A node instance</param>
    /// <returns>true if the node is Null Object otherwise false.</returns>
    /// <seealso cref="NullAstNode"/>
    /// <seealso cref="NullAstExpressionNode"/>
    /// <seealso cref="NullAstExpressionNode"/>
    public static bool IsNull( this IAstNode? node )
        => node is null
            or NullAstNode
            or NullAstExpressionNode
            or NullAstInitializerNode;

    /// <summary>
    /// Check if the node is not Null Object
    /// </summary>
    /// <param name="node">A node instance</param>
    /// <returns>true if the node is Null Object otherwise false.</returns>
    /// <seealso cref="NullAstNode"/>
    /// <seealso cref="NullAstExpressionNode"/>
    /// <seealso cref="NullAstExpressionNode"/>
    public static bool IsNotNull( this IAstNode? node )
        => !node.IsNull();
}
