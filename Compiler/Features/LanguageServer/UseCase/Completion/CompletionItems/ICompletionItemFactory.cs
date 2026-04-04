using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Completion;
using KSPCompiler.Shared.Domain.Compilation.Symbols;

namespace KSPCompiler.Features.LanguageServer.UseCase.Completion.CompletionItems;

public interface ICompletionItemFactory<in TSymbol> where TSymbol : SymbolBase
{
    CompletionItem Create( TSymbol symbol, string partialName, bool preferSnippetInsertion );

    bool TryCreateFixedSnippet( string partialName, bool preferSnippetInsertion, out CompletionItem result )
    {
        result = null!;
        return false;
    }
}
