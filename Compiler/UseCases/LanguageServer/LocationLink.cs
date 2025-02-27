using KSPCompiler.Commons.Text;

namespace KSPCompiler.UseCases.LanguageServer;

public sealed record LocationLink
{
    public ScriptLocation Location { get; init; } = ScriptLocation.Empty;
    public Position Range { get; init; }
}
