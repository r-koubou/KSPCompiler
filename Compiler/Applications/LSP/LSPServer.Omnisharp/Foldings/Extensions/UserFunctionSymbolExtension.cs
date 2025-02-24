using KSPCompiler.Domain.Symbols;

using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace KSPCompiler.Applications.LSPServer.Omnisharp.Foldings.Extensions;

public static class UserFunctionSymbolExtension
{
    public static FoldingRange AsFoldingRange( this UserFunctionSymbol self )
        => new()
        {
            StartLine      = self.Range.BeginLine.Value - 1,
            StartCharacter = self.Range.BeginColumn.Value,
            EndLine        = self.Range.EndLine.Value - 1,
            EndCharacter   = self.Range.EndColumn.Value,
            Kind           = FoldingRangeKind.Region
        };
}
