using KSPCompiler.Commons.Text;

namespace KSPCompiler.UseCases.LanguageServer.Renaming;

public sealed record RenamingItem
{
    public string NewText { get; init; } = null!;
    public Position Range { get; init; }
}
