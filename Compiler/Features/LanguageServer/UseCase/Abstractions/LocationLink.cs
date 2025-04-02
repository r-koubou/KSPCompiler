using KSPCompiler.Shared.Text;

namespace KSPCompiler.Features.LanguageServer.UseCase.Abstractions;

public sealed record LocationLink
{
    public ScriptLocation Location { get; init; } = null!;
    public Position Range { get; init; }
}
