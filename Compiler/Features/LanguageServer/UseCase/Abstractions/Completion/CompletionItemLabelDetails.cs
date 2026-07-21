namespace KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Completion;

public sealed record CompletionItemLabelDetails(
    string? Detail,
    string? Description
);
