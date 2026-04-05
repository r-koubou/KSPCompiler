namespace KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.Completion;

public record struct CompletionHandlerConfig
{
    public bool PreferSnippetInsertion { get; init; }

    public static CompletionHandlerConfig Default
        => new()
        {
            PreferSnippetInsertion = false
        };
}
