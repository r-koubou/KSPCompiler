using KSPCompiler.Domain.Symbols;

using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace KSPCompiler.LSPServer.Core.Foldings.Extensions;

public static class CallbackSymbolExtension
{
    public static FoldingRange AsFoldingRange( this CallbackSymbol self )
        => new()
        {
            StartLine      = self.Range.BeginLine.Value - 1,
            StartCharacter = self.Range.BeginColumn.Value,
            EndLine        = self.Range.EndLine.Value - 1,
            EndCharacter   = self.Range.EndColumn.Value,
            Kind           = FoldingRangeKind.Region
        };
}
