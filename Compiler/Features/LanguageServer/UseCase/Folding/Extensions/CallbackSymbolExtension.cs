using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Folding;
using KSPCompiler.Shared.Domain.Compilation.Symbols;

namespace KSPCompiler.Features.LanguageServer.UseCase.Folding.Extensions;

public static class CallbackSymbolExtension
{
    public static FoldingItem AsFoldingRange( this CallbackSymbol self )
        => new()
        {
            Position = self.Range,
            Kind = FoldingRangeKind.Region
        };
}
