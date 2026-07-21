using System.Text;

using KSPCompiler.Features.LanguageServer.UseCase.Abstractions;
using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Completion;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData;
using KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData.Extensions;

namespace KSPCompiler.Features.LanguageServer.UseCase.Completion.CompletionItems;

public sealed class UICompletionItemFactory(
    StringBuilder? snippetTextBuilder
) : ICompletionItemFactory<UITypeSymbol>
{
    private const string Detail = "UI";
    private const CompletionItemKind Kind = CompletionItemKind.Class;

    private readonly StringBuilder snippetTextBuilder = snippetTextBuilder ?? new StringBuilder();

    public CompletionItem Create( UITypeSymbol symbol, string partialName, bool preferSnippetInsertion )
    {
        snippetTextBuilder.Clear();

        if( TryCreateCommandSnippet( symbol, snippetTextBuilder, preferSnippetInsertion, out var snippetResult ) )
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

    private static bool TryCreateCommandSnippet( UITypeSymbol symbol, StringBuilder stringBuilder, bool preferSnippetInsertion, out CompletionItem result )
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
        stringBuilder.Clear();

        // declare ui_**** <name>

        if( !DataTypeUtility.TryGuessKspTypeCharacter( symbol.DataType, out var kspTypeCharacter ) )
        {
            kspTypeCharacter = "";
        }

        stringBuilder.Append( "declare " ).Append( symbol.Name.Value ).Append( " " );
        stringBuilder.Append( $"{kspTypeCharacter}" ).Append( "${" ).Append( placeholderIndex ).Append( ":name}" );
        placeholderIndex++;

        if( symbol.InitializerArguments.Count == 0 )
        {
            result = new CompletionItem(
                Label: symbol.Name.Value,
                LabelDetails: null,
                Kind: Kind,
                Detail: Detail,
                Documentation: document,
                InsertTextFormat: InsertTextFormat.Snippet,
                InsertText: stringBuilder.ToString()
            );

            return true;
        }

        // [array-size]

        if( symbol.DataType.IsArray() )
        {
            stringBuilder.Append( "[${" ).Append( placeholderIndex ).Append( ":array-size}]" );
            placeholderIndex++;
        }

        // Initializer

        var argCount = symbol.InitializerArguments.Count;

        if( argCount > 0 )
        {
            stringBuilder.Append( " (" );
        }

        var argIndex = 0;

        foreach( var arg in symbol.InitializerArguments )
        {
            stringBuilder.Append( "${" )
                         .Append( placeholderIndex ).Append( ":" )
                         .Append( arg.Name.Value )
                         .Append( "}" );

            if( argIndex + 1 < argCount )
            {
                stringBuilder.Append( ", " );
            }

            argIndex++;
            placeholderIndex++;
        }

        stringBuilder.Append( ")" );

        result = new CompletionItem(
            Label: symbol.Name.Value,
            LabelDetails: null,
            Kind: Kind,
            Detail: Detail,
            Documentation: document,
            InsertTextFormat: InsertTextFormat.Snippet,
            InsertText: stringBuilder.ToString()
        );

        return true;
    }
}
