namespace KSPCompiler.Domain.Ast.Nodes;

/// <summary>
/// A token that can be used to abort a traversal ast nodes.
/// </summary>
public sealed class AbortTraverseToken
{
    private bool aborted;

    public bool Aborted
        => aborted;

    public void Abort()
    {
        aborted = true;
    }
}
