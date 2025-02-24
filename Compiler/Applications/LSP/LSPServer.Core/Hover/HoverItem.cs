namespace KSPCompiler.Applications.LSPServer.Core.Hover;

public sealed record HoverItem
{
    public StringOrMarkdownContent Content { get; init; } = null!;
}
