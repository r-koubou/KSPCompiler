using KSPCompiler.Domain.Symbols;
using KSPCompiler.UseCases.LanguageServer.Folding;

namespace KSPCompiler.Interactors.LanguageServer.Folding.Extensions;

public static class UserFunctionSymbolExtension
{
    public static FoldingItem AsFoldingRange( this UserFunctionSymbol self )
        => new()
        {
            Position = self.Range,
            Kind     = FoldingRangeKind.Region
        };
}
