namespace KSPCompiler.Features.Compilation.Domain.Ast.Nodes.Blocks;

public sealed class NullAstCaseBlockNode : AstCaseBlock
{
    public static readonly NullAstCaseBlockNode Instance = new();
    private NullAstCaseBlockNode()
        : base(
            NullAstNode.Instance,
            NullAstExpressionNode.Instance,
            NullAstExpressionNode.Instance,
            new AstBlockNode()
        ) {}
}
