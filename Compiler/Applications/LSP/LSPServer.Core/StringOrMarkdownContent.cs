namespace KSPCompiler.Applications.LSPServer.Core;

public sealed record  StringOrMarkdownContent
{
    public bool IsMarkdown { get; init; }
    public string Value { get; init; } = null!;
}
