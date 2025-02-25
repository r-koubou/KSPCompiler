namespace KSPCompiler.UseCases.LanguageServer.Completion;

public sealed record CompletionItem(
    string Label,
    CompletionItemKind Kind,
    string? Detail,
    InsertTextFormat InsertTextFormat,
    string? Documentation,
    string? InsertText
);
