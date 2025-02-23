using KSPCompiler.Commons.Text;

namespace KSPCompiler.Applications.LSPServer.CoreNew;

public sealed record LocationLink
{
    public ScriptLocation Location { get; init; } = ScriptLocation.Empty;
    public Position Range { get; init; }
}
