namespace KSPCompiler.Applications.LSPServer.CoreNew;

public sealed record  StringOrMarkdownContent
{
    public bool IsMarkdown { get; init; }
    public string Value { get; init; } = null!;
}
