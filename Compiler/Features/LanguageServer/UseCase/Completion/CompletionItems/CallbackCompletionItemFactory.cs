using System;
using System.Runtime.CompilerServices;

using KSPCompiler.Features.LanguageServer.UseCase.Abstractions;
using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Completion;
using KSPCompiler.Shared.Domain.Compilation.Symbols;

namespace KSPCompiler.Features.LanguageServer.UseCase.Completion.CompletionItems;

public sealed class CallbackCompletionItemFactory : ICompletionItemFactory<CallbackSymbol>
{
    private const string Detail = "Callback";
    private const CompletionItemKind Kind = CompletionItemKind.Event;

    public CompletionItem Create( CallbackSymbol symbol, string partialName, bool _ )
    {
        if( TryCreateSnippetItem( symbol, partialName, out var completionItem ) )
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

    private static bool TryCreateSnippetItem( CallbackSymbol callbackSymbol, string partialName, out CompletionItem result )
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
            var stringHandler = new DefaultInterpolatedStringHandler( 0, 0 );

            stringHandler.AppendLiteral( "on " );
            stringHandler.AppendFormatted( callbackSymbol.Name.Value );
            stringHandler.AppendLiteral( "(" );

            for( var snippetIndex = 0; snippetIndex < callbackSymbol.ArgumentCount; snippetIndex++ )
            {
                stringHandler.AppendLiteral( "${" );
                stringHandler.AppendFormatted( snippetIndex + 1 );
                stringHandler.AppendLiteral( ":" );
                stringHandler.AppendFormatted( callbackSymbol.Arguments[ snippetIndex ].Name.Value );
                stringHandler.AppendLiteral( "}" );

                if( snippetIndex < callbackSymbol.ArgumentCount - 1 )
                {
                    stringHandler.AppendLiteral( ", " );
                }
            }

            var codeIndex = callbackSymbol.ArgumentCount + 1;

            stringHandler.AppendLiteral( ")" );
            stringHandler.AppendLiteral( LspConstants.NewLine );
            stringHandler.AppendLiteral( "    ${" );
            stringHandler.AppendFormatted( codeIndex );
            stringHandler.AppendLiteral( ":code}" );
            stringHandler.AppendLiteral( LspConstants.NewLine );
            stringHandler.AppendLiteral( "end on" );
            stringHandler.AppendLiteral( LspConstants.NewLine );

            result = new CompletionItem(
                Label: $"on {callbackSymbol.Name.Value}",
                LabelDetails: null,
                Kind: Kind,
                Detail: Detail,
                Documentation: document,
                InsertTextFormat: InsertTextFormat.Snippet,
                InsertText: stringHandler.ToStringAndClear()
            );

            return true;
        }
        else
        {
            var stringHandler = new DefaultInterpolatedStringHandler( 0, 0 );

            stringHandler.AppendLiteral( "on " );
            stringHandler.AppendFormatted( callbackSymbol.Name.Value );
            stringHandler.AppendLiteral( LspConstants.NewLine );
            stringHandler.AppendLiteral( "    ${1:code}" );
            stringHandler.AppendLiteral( LspConstants.NewLine );
            stringHandler.AppendLiteral( "end on" );
            stringHandler.AppendLiteral( LspConstants.NewLine );

            result = new CompletionItem(
                Label: $"on {callbackSymbol.Name.Value}",
                LabelDetails: null,
                Kind: Kind,
                Detail: Detail,
                Documentation: document,
                InsertTextFormat: InsertTextFormat.Snippet,
                InsertText: stringHandler.ToStringAndClear()
            );

            return true;
        }
    }

    public bool TryCreateFixedSnippet( string partialName, out CompletionItem result )
    {
        result = null!;

        if( !partialName.StartsWith( 'o' ) )
        {
            return false;
        }

        var stringHandler = new DefaultInterpolatedStringHandler( 0, 0 );

        stringHandler.AppendLiteral( "on ${1:name}" );
        stringHandler.AppendLiteral( LspConstants.NewLine );
        stringHandler.AppendLiteral( @"    ${2:{TODO: your script here\}}" );
        stringHandler.AppendLiteral( LspConstants.NewLine );
        stringHandler.AppendLiteral( "end on" );
        stringHandler.AppendLiteral( LspConstants.NewLine );

        result = new CompletionItem(
            Label: "on <name>",
            LabelDetails: null,
            Kind: CompletionItemKind.Snippet,
            Detail: Detail,
            Documentation: string.Empty,
            InsertTextFormat: InsertTextFormat.Snippet,
            InsertText: stringHandler.ToStringAndClear()
        );

        return true;
    }
}
