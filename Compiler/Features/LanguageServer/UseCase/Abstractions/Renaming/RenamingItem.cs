using KSPCompiler.Shared.Text;

namespace KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Renaming;

public sealed record RenamingItem
{
    public string NewText { get; init; } = null!;
    public Position Range { get; init; }
}
