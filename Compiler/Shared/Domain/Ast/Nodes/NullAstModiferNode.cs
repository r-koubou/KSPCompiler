namespace KSPCompiler.Features.Compilation.Domain.Ast.Nodes;

public sealed class NullAstModiferNode : AstModiferNode
{
    public static readonly NullAstModiferNode Instance = new();
    private NullAstModiferNode() {}
}
