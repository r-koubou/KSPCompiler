using System.Runtime.CompilerServices;

using KSPCompiler.Features.LanguageServer.UseCase.Abstractions;
using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Completion;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData;
using KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData.Extensions;

namespace KSPCompiler.Features.LanguageServer.UseCase.Completion.CompletionItems;

public sealed class UICompletionItemFactory : ICompletionItemFactory<UITypeSymbol>
{
    private const string Detail = "UI";
    private const CompletionItemKind Kind = CompletionItemKind.Class;

    public CompletionItem Create( UITypeSymbol symbol, string partialName, bool preferSnippetInsertion )
    {
        if( TryCreateCommandSnippet( symbol, preferSnippetInsertion, out var snippetResult ) )
        {
            return snippetResult;
        }

        var document = DocumentUtility.GetCommentOrDescriptionText( symbol );

        return new CompletionItem(
            Label: symbol.Name.Value,
            LabelDetails: null,
            Kind: Kind,
            Detail: Detail,
            InsertTextFormat: InsertTextFormat.PlainText,
            Documentation: document,
            InsertText: symbol.Name.Value
        );
    }

    private static bool TryCreateCommandSnippet( UITypeSymbol symbol, bool preferSnippetInsertion, out CompletionItem result )
    {
        result = null!;

        if( !preferSnippetInsertion )
        {
            return false;
        }

        // case 1: Non-Array & Non-Initializer     ui_**** [$|~|@]${1:name}
        // case 2: Non-Array & Initializer         ui_**** [$|~|@]${1:name}(args...)
        // case 3: Array & Non-Initializer         ui_**** [%|?|!]${1:name}[${2:array-size}]
        // case 3: Array & Initializer             ui_**** [%|?|!]${1:name}[${2:array-size}](args...)

        var document = DocumentUtility.GetCommentOrDescriptionText( symbol );
        document = string.IsNullOrEmpty( document ) ? null : document;

        var placeholderIndex = 1;

        // declare ui_**** <name>

        if( !DataTypeUtility.TryGuessKspTypeCharacter( symbol.DataType, out var kspTypeCharacter ) )
        {
            kspTypeCharacter = "";
        }

        var isArray = symbol.DataType.IsArray();
        var argCount = symbol.InitializerArguments.Count;
        var stringHandler = new DefaultInterpolatedStringHandler( 0, 0 );

        stringHandler.AppendLiteral( "declare " );
        stringHandler.AppendFormatted( symbol.Name.Value );
        stringHandler.AppendLiteral( " " );
        stringHandler.AppendFormatted( kspTypeCharacter );
        stringHandler.AppendLiteral( "${" );
        stringHandler.AppendFormatted( placeholderIndex );
        stringHandler.AppendLiteral( ":name}" );
        placeholderIndex++;

        if( argCount == 0 )
        {
            result = new CompletionItem(
                Label: symbol.Name.Value,
                LabelDetails: null,
                Kind: Kind,
                Detail: Detail,
                Documentation: document,
                InsertTextFormat: InsertTextFormat.Snippet,
                InsertText: stringHandler.ToStringAndClear()
            );

            return true;
        }

        // [array-size]

        if( isArray )
        {
            stringHandler.AppendLiteral( "[${" );
            stringHandler.AppendFormatted( placeholderIndex );
            stringHandler.AppendLiteral( ":array-size}]" );
            placeholderIndex++;
        }

        // Initializer

        stringHandler.AppendLiteral( " (" );

        var argIndex = 0;

        foreach( var arg in symbol.InitializerArguments )
        {
            stringHandler.AppendLiteral( "${" );
            stringHandler.AppendFormatted( placeholderIndex );
            stringHandler.AppendLiteral( ":" );
            stringHandler.AppendFormatted( arg.Name.Value );
            stringHandler.AppendLiteral( "}" );

            if( argIndex + 1 < argCount )
            {
                stringHandler.AppendLiteral( ", " );
            }

            argIndex++;
            placeholderIndex++;
        }

        stringHandler.AppendLiteral( ")" );

        result = new CompletionItem(
            Label: symbol.Name.Value,
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
