using System.Text;

using KSPCompiler.Features.LanguageServer.UseCase.Abstractions;
using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Completion;
using KSPCompiler.Shared.Domain.Compilation.Symbols;

namespace KSPCompiler.Features.LanguageServer.UseCase.Completion.CompletionItems;

public sealed class UserFunctionCompletionItemFactory(
    StringBuilder? snippetTextBuilder
) : ICompletionItemFactory<UserFunctionSymbol>
{
    private const string Detail = "User Function";

    private readonly StringBuilder snippetTextBuilder = snippetTextBuilder ?? new StringBuilder();

    public CompletionItem Create( UserFunctionSymbol symbol, string partialName, bool preferSnippetInsertion )
    {
        snippetTextBuilder.Clear();

        if( TryCreateSnippetItem( symbol, partialName, snippetTextBuilder, out var completionItem ) )
        {
            return completionItem;
        }

        return new CompletionItem(
            Label: symbol.Name.Value,
            Kind: CompletionItemKind.Function,
            Detail: Detail,
            Documentation: DocumentUtility.GetCommentOrDescriptionText( symbol ),
            InsertTextFormat: InsertTextFormat.PlainText,
            InsertText: symbol.Name.Value
        );
    }

    private static bool TryCreateSnippetItem( UserFunctionSymbol callbackSymbol, string partialName, StringBuilder stringBuilder, out CompletionItem result )
    {
        var document = DocumentUtility.GetCommentOrDescriptionText( callbackSymbol );
        document = string.IsNullOrEmpty( document ) ? null : document;

        result = null!;

        if( !partialName.StartsWith( "function " ) )
        {
            return false;
        }

        stringBuilder.Append( "function " ).AppendLine( "${1:name}" )
                     .AppendLine( "    ${2:code}" )
                     .AppendLine( "end function" );

        result = new CompletionItem(
            Label: "function",
            Kind: CompletionItemKind.Snippet,
            Detail: Detail,
            Documentation: document,
            InsertTextFormat: InsertTextFormat.Snippet,
            InsertText: stringBuilder.ToString()
        );

        return true;
    }
}
