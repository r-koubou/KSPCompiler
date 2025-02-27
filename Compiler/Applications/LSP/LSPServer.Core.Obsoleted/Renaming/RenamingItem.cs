using KSPCompiler.Commons.Text;

namespace KSPCompiler.Applications.LSPServer.Core.Renaming;

public sealed record RenamingItem
{
    public string NewText { get; init; } = null!;
    public Position Range { get; init; }
}
