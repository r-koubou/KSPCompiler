using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Folding;
using KSPCompiler.Shared.Domain.Compilation.Symbols;

namespace KSPCompiler.Features.LanguageServer.UseCase.Folding.Extensions;

public static class UserFunctionSymbolExtension
{
    public static FoldingItem AsFoldingRange( this UserFunctionSymbol self )
        => new()
        {
            Position = self.Range,
            Kind     = FoldingRangeKind.Region
        };
}
