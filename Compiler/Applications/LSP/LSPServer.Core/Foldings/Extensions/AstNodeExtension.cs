using OmniSharp.Extensions.LanguageServer.Protocol.Models;

using KspPosition = KSPCompiler.Commons.Text.Position;

namespace KSPCompiler.Applications.LSPServer.Core.Foldings.Extensions;

public static class AstNodeExtension
{
    public static FoldingRange AsFoldingRange( this KspPosition self )
        => new()
        {
            StartLine      = self.BeginLine.Value - 1,
            StartCharacter = self.BeginColumn.Value,
            EndLine        = self.EndLine.Value - 1,
            EndCharacter   = self.EndColumn.Value,
            Kind           = FoldingRangeKind.Region
        };
}
