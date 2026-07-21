using KSPCompiler.Features.LanguageServer.UseCase.Abstractions;
using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Completion;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData.Extensions;

namespace KSPCompiler.Features.LanguageServer.UseCase.Completion.CompletionItems;

public sealed class VariableCompletionItemFactory : ICompletionItemFactory<VariableSymbol>
{
    public CompletionItem Create( VariableSymbol symbol, string partialName, bool _ )
    {
        var insertText = symbol.Name.Value;
        var kind = CompletionItemKind.Variable;

        var document = DocumentUtility.GetCommentOrDescriptionText( symbol );
        document = string.IsNullOrEmpty( document ) ? null : document;

        if( !symbol.BuiltIn && symbol.Modifier.IsConstant() )
        {
            kind = CompletionItemKind.Constant;
        }

        return new CompletionItem(
            Label: symbol.Name.Value,
            LabelDetails: null,
            Kind: kind,
            Detail: symbol.BuiltIn ? "Built-in Variable" : "User Variable",
            Documentation: document,
            InsertTextFormat: InsertTextFormat.PlainText,
            InsertText: insertText
        );
    }
}
