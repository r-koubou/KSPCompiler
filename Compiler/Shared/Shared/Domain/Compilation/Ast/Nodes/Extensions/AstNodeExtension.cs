using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Blocks;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Statements;

namespace KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Extensions;

public static class AstNodeExtension
{
    /// <summary>
    /// Check if the node is Null Object
    /// </summary>
    /// <param name="node">A node instance</param>
    /// <returns>true if the node is Null Object otherwise false.</returns>
    /// <seealso cref="NullAstNode"/>
    /// <seealso cref="NullAstModiferNode"/>
    /// <seealso cref="NullAstExpressionNode"/>
    /// <seealso cref="NullAstExpressionListNode"/>
    /// <seealso cref="NullAstVariableInitializerNode"/>
    /// <seealso cref="NullAstPrimitiveInitializerNode"/>
    /// <seealso cref="NullAstArrayInitializerNode"/>
    public static bool IsNull( this IAstNode? node )
        => node is null
            or NullAstNode
            or NullAstModiferNode
            or NullAstBlockNode
            or NullAstCaseBlockNode
            or NullAstExpressionNode
            or NullAstExpressionListNode
            or NullAstVariableInitializerNode
            or NullAstPrimitiveInitializerNode
            or NullAstArrayInitializerNode;

    /// <summary>
    /// Check if the node is not Null Object
    /// </summary>
    /// <param name="node">A node instance</param>
    /// <returns>true if the node is Null Object otherwise false.</returns>
    /// <seealso cref="IsNull"/>
    public static bool IsNotNull( this IAstNode? node )
        => !node.IsNull();
}
