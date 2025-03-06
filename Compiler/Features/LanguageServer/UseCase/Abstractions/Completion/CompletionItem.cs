namespace KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Completion;

public sealed record CompletionItem(
    string Label,
    CompletionItemKind Kind,
    string? Detail,
    InsertTextFormat InsertTextFormat,
    string? Documentation,
    string? InsertText
);
