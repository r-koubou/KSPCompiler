namespace KSPCompiler.Domain.Ast.Nodes.Expressions;

public sealed class NullAstExpressionListNode : AstExpressionListNode
{
    public static NullAstExpressionListNode Instance { get; } = new NullAstExpressionListNode();

    private NullAstExpressionListNode()
        : base( NullAstNode.Instance ) {}
}
