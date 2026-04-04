using KSPCompiler.Features.LanguageServer.UseCase.Abstractions;
using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Completion;
using KSPCompiler.Shared.Domain.Compilation.Symbols;

namespace KSPCompiler.Features.LanguageServer.UseCase.Completion.CompletionItems;

public sealed class PgsKeyCompletionItemFactory : ICompletionItemFactory<PgsSymbol>
{
    public CompletionItem Create( PgsSymbol symbol, string partialName, bool preferSnippetInsertion )
    {
        var document = DocumentUtility.GetCommentOrDescriptionText( symbol );
        document = string.IsNullOrEmpty( document ) ? null : document;

        return new CompletionItem(
            Label: symbol.Name.Value,
            Kind: CompletionItemKind.Variable,
            Detail: "PGS Key-Id",
            Documentation: document,
            InsertTextFormat: InsertTextFormat.PlainText,
            InsertText: symbol.Name.Value
        );
    }
}
