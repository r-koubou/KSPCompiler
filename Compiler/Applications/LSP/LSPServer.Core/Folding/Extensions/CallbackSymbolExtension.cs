using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Applications.LSPServer.Core.Folding.Extensions;

public static class CallbackSymbolExtension
{
    public static FoldingItem AsFoldingRange( this CallbackSymbol self )
        => new()
        {
            Position = self.Range,
            Kind = FoldingRangeKind.Region
        };
}
