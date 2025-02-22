namespace KSPCompiler.Applications.LSPServer.CoreNew.Completion;

public sealed record CompletionItem(
    string Label,
    CompletionItemKind Kind,
    string? Detail,
    InsertTextFormat InsertTextFormat,
    string? Documentation,
    string? InsertText
);
