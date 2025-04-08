namespace KSPCompiler.Features.LanguageServer.UseCase.Abstractions;

public sealed record  StringOrMarkdownContent
{
    public bool IsMarkdown { get; init; }
    public string Value { get; init; } = null!;
}
