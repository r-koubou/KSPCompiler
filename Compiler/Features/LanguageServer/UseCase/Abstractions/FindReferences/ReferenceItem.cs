using KSPCompiler.Shared.Text;

namespace KSPCompiler.Features.LanguageServer.UseCase.Abstractions.FindReferences;

public sealed record ReferenceItem
{
    public ScriptLocation Location { get; init; } = null!;
    public Position Range { get; init; }
}
