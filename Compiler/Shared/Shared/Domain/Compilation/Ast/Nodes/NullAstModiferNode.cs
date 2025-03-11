namespace KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;

public sealed class NullAstModiferNode : AstModiferNode
{
    public static readonly NullAstModiferNode Instance = new();
    private NullAstModiferNode() {}
}
