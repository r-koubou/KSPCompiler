namespace KSPCompiler.Features.Compilation.Domain.Ast.Nodes.Blocks;

public sealed class NullAstBlockNode : AstBlockNode
{
    public static readonly NullAstBlockNode Instance = new();
    private NullAstBlockNode() {}
}
