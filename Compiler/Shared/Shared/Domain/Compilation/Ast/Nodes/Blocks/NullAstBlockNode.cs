namespace KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Blocks;

public sealed class NullAstBlockNode : AstBlockNode
{
    public static readonly NullAstBlockNode Instance = new();
    private NullAstBlockNode() {}
}
