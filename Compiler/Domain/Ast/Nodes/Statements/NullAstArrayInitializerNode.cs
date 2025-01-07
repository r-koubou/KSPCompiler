using KSPCompiler.Commons.Text;
using KSPCompiler.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Domain.Ast.Nodes.Statements;

public sealed class NullAstArrayInitializerNode : AstArrayInitializerNode
{
    public static readonly AstArrayInitializerNode Instance = new NullAstArrayInitializerNode();

    /// <summary>
    /// Always return <see cref="AstNodeId.ArrayInitializer"/>.
    /// </summary>
    public override AstNodeId Id
        => AstNodeId.ArrayInitializer;

    /// <summary>
    /// Always return <see cref="NullAstExpressionNode.Instance"/>.
    /// </summary>
    public override AstExpressionNode Size
        => NullAstExpressionNode.Instance;

    /// <summary>
    /// Always return <see cref="NullAstExpressionListNode.Instance"/>.
    /// </summary>
    public override AstExpressionListNode Initializer
        => NullAstExpressionListNode.Instance;

    public override bool HasAssignOperator
        => false;

    /// <summary>
    /// Always return zero and the set is ignored.
    /// </summary>
    public override Position Position
        => Position.Zero;

    /// <summary>
    /// Always return this instance and the set is ignored.
    /// </summary>
    public override IAstNode Parent
        => this;

    private NullAstArrayInitializerNode() {}
}
