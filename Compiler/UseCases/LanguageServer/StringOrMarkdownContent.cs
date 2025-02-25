namespace KSPCompiler.UseCases.LanguageServer;

public sealed record  StringOrMarkdownContent
{
    public bool IsMarkdown { get; init; }
    public string Value { get; init; } = null!;
}
