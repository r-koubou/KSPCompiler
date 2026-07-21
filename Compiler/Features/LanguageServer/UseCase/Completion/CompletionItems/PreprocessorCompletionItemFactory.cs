using KSPCompiler.Features.LanguageServer.UseCase.Abstractions;
using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Completion;
using KSPCompiler.Shared.Domain.Compilation.Symbols;

namespace KSPCompiler.Features.LanguageServer.UseCase.Completion.CompletionItems;

public sealed class PreprocessorCompletionItemFactory : ICompletionItemFactory<PreProcessorSymbol>
{
    public CompletionItem Create( PreProcessorSymbol symbol, string partialName, bool _ )
    {
        var document = DocumentUtility.GetCommentOrDescriptionText( symbol );
        document = string.IsNullOrEmpty( document ) ? null : document;

        return new CompletionItem(
            Label: symbol.Name.Value,
            LabelDetails: null,
            Kind: CompletionItemKind.Variable,
            Detail: "Preprocessor",
            Documentation: document,
            InsertTextFormat: InsertTextFormat.PlainText,
            InsertText: symbol.Name.Value
        );
    }
}
