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

        return new CompletionItem(
            Label: symbol.Name.Value,
            Kind: CompletionItemKind.Function,
            Detail: Detail,
            Documentation: DocumentUtility.GetCommentOrDescriptionText( symbol ),
            InsertTextFormat: InsertTextFormat.PlainText,
            InsertText: symbol.Name.Value
        );
    }

    public bool TryCreateFixedSnippet( string partialName, out CompletionItem result )
    {
        result = null!;

        if( !partialName.StartsWith( "f" ) )
        {
            return false;
        }

        snippetTextBuilder.Append( "function " ).AppendLine( "${1:name}" )
                          .AppendLine( "    ${2:code}" )
                          .AppendLine( "end function" );

        result = new CompletionItem(
            Label: "function <name>",
            Kind: CompletionItemKind.Snippet,
            Detail: Detail,
            Documentation: string.Empty,
            InsertTextFormat: InsertTextFormat.Snippet,
            InsertText: snippetTextBuilder.ToString()
        );

        return true;
    }
}
