using KSPCompiler.Domain.Symbols;
using KSPCompiler.UseCases.LanguageServer.Folding;

namespace KSPCompiler.Interactors.LanguageServer.Folding.Extensions;

public static class CallbackSymbolExtension
{
    public static FoldingItem AsFoldingRange( this CallbackSymbol self )
        => new()
        {
            Position = self.Range,
            Kind = FoldingRangeKind.Region
        };
}
