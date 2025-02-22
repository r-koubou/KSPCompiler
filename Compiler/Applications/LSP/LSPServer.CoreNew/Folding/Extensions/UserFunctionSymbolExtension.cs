using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Applications.LSPServer.CoreNew.Folding.Extensions;

public static class UserFunctionSymbolExtension
{
    public static FoldingItem AsFoldingRange( this UserFunctionSymbol self )
        => new()
        {
            Position = self.Range,
            Kind     = FoldingRangeKind.Region
        };
}
