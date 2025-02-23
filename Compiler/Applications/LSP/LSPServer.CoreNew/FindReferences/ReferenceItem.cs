using KSPCompiler.Commons.Text;

namespace KSPCompiler.Applications.LSPServer.CoreNew.FindReferences;

public sealed record ReferenceItem
{
    public ScriptLocation Location { get; init; } = null!;
    public Position Range { get; init; }
}
