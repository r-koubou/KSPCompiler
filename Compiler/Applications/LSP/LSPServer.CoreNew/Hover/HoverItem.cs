namespace KSPCompiler.Applications.LSPServer.CoreNew.Hover;

public sealed record HoverItem
{
    public string Content { get; init; } = null!;
    public bool IsMarkdown { get; init; }
}
