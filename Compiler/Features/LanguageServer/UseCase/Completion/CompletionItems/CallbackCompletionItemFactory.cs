using System.Text;

using KSPCompiler.Features.LanguageServer.UseCase.Abstractions;
using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Completion;
using KSPCompiler.Shared.Domain.Compilation.Symbols;

namespace KSPCompiler.Features.LanguageServer.UseCase.Completion.CompletionItems;

public sealed class CallbackCompletionItemFactory(
    StringBuilder? snippetTextBuilder
) : ICompletionItemFactory<CallbackSymbol>
{
    private const string Detail = "Callback";
    private const CompletionItemKind Kind = CompletionItemKind.Event;

    private readonly StringBuilder snippetTextBuilder = snippetTextBuilder ?? new StringBuilder();

    public CompletionItem Create( CallbackSymbol symbol, string partialName, bool _ )
    {
        snippetTextBuilder.Clear();

        if( TryCreateSnippetItem( symbol, partialName, snippetTextBuilder, out var completionItem ) )
        {
            return completionItem;
        }

        return new CompletionItem(
            Label: symbol.Name.Value,
            LabelDetails: null,
            Kind: Kind,
            Detail: Detail,
            Documentation: DocumentUtility.GetCommentOrDescriptionText( symbol ),
            InsertTextFormat: InsertTextFormat.PlainText,
            InsertText: symbol.Name.Value
        );
    }

    private static bool TryCreateSnippetItem( CallbackSymbol callbackSymbol, string partialName, StringBuilder stringBuilder, out CompletionItem result )
    {
        var document = DocumentUtility.GetCommentOrDescriptionText( callbackSymbol );
        document = string.IsNullOrEmpty( document ) ? null : document;

        result = null!;

        if( !partialName.StartsWith( "o" ) )
        {
            return false;
        }

        if( callbackSymbol.ArgumentCount > 0 )
        {
            stringBuilder.Append( "on " ).Append( callbackSymbol.Name.Value ).Append( '(' );

            for( var snippetIndex = 0; snippetIndex < callbackSymbol.ArgumentCount; snippetIndex++ )
            {
                stringBuilder.Append( "${" )
                             .Append( snippetIndex + 1 ).Append( ':' )
                             .Append( callbackSymbol.Arguments[ snippetIndex ].Name.Value )
                             .Append( '}' );

                if( snippetIndex < callbackSymbol.ArgumentCount - 1 )
                {
                    stringBuilder.Append( ", " );
                }
            }

            var codeIndex = callbackSymbol.ArgumentCount + 1;

            stringBuilder.AppendLine( ")" )
                         .AppendLine( $"    ${{{codeIndex}:code}}" )
                         .AppendLine( "end on" );
        }
        else
        {
            stringBuilder.Append( "on " ).AppendLine( callbackSymbol.Name.Value )
                         .AppendLine( "    ${1:code}" )
                         .AppendLine( "end on" );
        }

        result = new CompletionItem(
            Label: $"on {callbackSymbol.Name.Value}",
            LabelDetails: null,
            Kind: Kind,
            Detail: Detail,
            Documentation: document,
            InsertTextFormat: InsertTextFormat.Snippet,
            InsertText: stringBuilder.ToString()
        );

        return true;
    }

    public bool TryCreateFixedSnippet( string partialName, out CompletionItem result )
    {
        result = null!;

        if( !partialName.StartsWith( "o" ) )
        {
            return false;
        }

        snippetTextBuilder.Clear();

        snippetTextBuilder.AppendLine( "on ${1:name}" )
                          .AppendLine( @"    ${2:{TODO: your script here\}}" )
                          .AppendLine( "end on" );

        result = new CompletionItem(
            Label: "on <name>",
            LabelDetails: null,
            Kind: CompletionItemKind.Snippet,
            Detail: Detail,
            Documentation: string.Empty,
            InsertTextFormat: InsertTextFormat.Snippet,
            InsertText: snippetTextBuilder.ToString()
        );

        return true;
    }
}
