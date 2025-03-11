namespace KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Hover;

public sealed record HoverItem
{
    public StringOrMarkdownContent Content { get; init; } = null!;
}
