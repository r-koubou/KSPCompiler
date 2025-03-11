using KSPCompiler.Shared.Text;

namespace KSPCompiler.Features.LanguageServer.UseCase.Abstractions;

public sealed record LocationLink
{
    public ScriptLocation Location { get; init; } = ScriptLocation.Empty;
    public Position Range { get; init; }
}
