namespace KSPCompiler.Applications.LSPServer.CoreNew.Hover;

public sealed record HoverItem
{
    public StringOrMarkdownContent Content { get; init; } = null!;
}
