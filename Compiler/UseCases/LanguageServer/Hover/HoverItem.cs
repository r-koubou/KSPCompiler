namespace KSPCompiler.UseCases.LanguageServer.Hover;

public sealed record HoverItem
{
    public StringOrMarkdownContent Content { get; init; } = null!;
}
