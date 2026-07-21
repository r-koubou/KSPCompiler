using System.Linq;
using System.Text;

using KSPCompiler.Features.LanguageServer.UseCase.Abstractions;
using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Completion;
using KSPCompiler.Shared.Domain.Compilation.Symbols;

namespace KSPCompiler.Features.LanguageServer.UseCase.Completion.CompletionItems;

public sealed class CommandCompletionItemFactory(
    StringBuilder? snippetTextBuilder
) : ICompletionItemFactory<CommandSymbol>
{
    private const string Detail = "Command";
    private const CompletionItemKind Kind = CompletionItemKind.Method;

    private readonly StringBuilder snippetTextBuilder = snippetTextBuilder ?? new StringBuilder();

    public CompletionItem Create( CommandSymbol symbol, string partialName, bool preferSnippetInsertion )
    {
        snippetTextBuilder.Clear();

        if( TryCreateCommandSnippet( symbol, snippetTextBuilder, preferSnippetInsertion, out var snippetResult ) )
        {
            return snippetResult;
        }

        var document = DocumentUtility.GetCommentOrDescriptionText( symbol );
        var labelDetail = CreateLabelDetail( symbol );

        return new CompletionItem(
            Label: symbol.Name.Value,
            LabelDetails: labelDetail,
            Kind: Kind,
            Detail: Detail,
            InsertTextFormat: InsertTextFormat.PlainText,
            Documentation: document,
            InsertText: symbol.Name.Value
        );
    }

    private static CompletionItemLabelDetails? CreateLabelDetail( CommandSymbol commandSymbol )
    {
        if( !TryCreateCommandArgumentsSignature( commandSymbol, out var signature ) )
        {
            return null;
        }

        return new CompletionItemLabelDetails(
            Detail: signature,
            Description: null
        );
    }

    private static bool TryCreateCommandArgumentsSignature( CommandSymbol commandSymbol, out string result )
    {
        result = string.Empty;

        if( commandSymbol.Arguments.Count == 0 )
        {
            return false;
        }

        var parameterStrings = commandSymbol.Arguments.Select( x => $"{x.Name.Value}" );

        result = $"({string.Join( ", ", parameterStrings )})";

        return true;
    }

    private static bool TryCreateCommandSnippet( CommandSymbol commandSymbol, StringBuilder stringBuilder, bool preferSnippetInsertion, out CompletionItem result )
    {
        result = null!;

        if( !preferSnippetInsertion )
        {
            return false;
        }

        var document = DocumentUtility.GetCommentOrDescriptionText( commandSymbol );
        document = string.IsNullOrEmpty( document ) ? null : document;

        if( commandSymbol.Arguments.Count == 0 )
        {
            result = new CompletionItem(
                Label: commandSymbol.Name.Value,
                LabelDetails: null,
                Kind: Kind,
                Detail: Detail,
                Documentation: document,
                InsertTextFormat: InsertTextFormat.PlainText,
                InsertText: commandSymbol.Name.Value
            );

            return true;
        }

        stringBuilder.Append( commandSymbol.Name.Value );
        stringBuilder.Append( "(" );

        var placeholderIndex = 1;
        var argCount = commandSymbol.Arguments.Count;

        foreach( var arg in commandSymbol.Arguments )
        {
            stringBuilder.Append( "${" )
                         .Append( placeholderIndex ).Append( ":" )
                         .Append( arg.Name.Value )
                         .Append( "}" );

            if( placeholderIndex < argCount )
            {
                stringBuilder.Append( ", " );
            }

            placeholderIndex++;
        }

        stringBuilder.Append( ")" );

        result = new CompletionItem(
            Label: commandSymbol.Name.Value,
            LabelDetails: CreateLabelDetail( commandSymbol ),
            Kind: Kind,
            Detail: Detail,
            Documentation: document,
            InsertTextFormat: InsertTextFormat.Snippet,
            InsertText: stringBuilder.ToString()
        );

        return true;
    }
}
