using System.Runtime.CompilerServices;

using KSPCompiler.Features.LanguageServer.UseCase.Abstractions;
using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Completion;
using KSPCompiler.Shared.Domain.Compilation.Symbols;

namespace KSPCompiler.Features.LanguageServer.UseCase.Completion.CompletionItems;

public sealed class UserFunctionCompletionItemFactory : ICompletionItemFactory<UserFunctionSymbol>
{
    private const string Detail = "User Function";

    public CompletionItem Create( UserFunctionSymbol symbol, string partialName, bool preferSnippetInsertion )
    {
        return new CompletionItem(
            Label: symbol.Name.Value,
            LabelDetails: null,
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

        if( !partialName.StartsWith( 'f' ) )
        {
            return false;
        }

        var stringHandler = new DefaultInterpolatedStringHandler( 0, 0 );

        stringHandler.AppendLiteral( "function " );
        stringHandler.AppendLiteral( "${1:name}" );
        stringHandler.AppendLiteral( LspConstants.NewLine );
        stringHandler.AppendLiteral( @"    ${2:{TODO: your script here\}}" );
        stringHandler.AppendLiteral( LspConstants.NewLine );
        stringHandler.AppendLiteral( "end function" );
        stringHandler.AppendLiteral( LspConstants.NewLine );

        result = new CompletionItem(
            Label: "function <name>",
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
