using System.Collections.Generic;

using KSPCompiler.Applications.LSPServer.CoreNew.Folding;

using FrameworkFoldingRange = EmmyLua.LanguageServer.Framework.Protocol.Message.FoldingRange.FoldingRange;
using FrameworkFoldingRangeKind = EmmyLua.LanguageServer.Framework.Protocol.Message.FoldingRange.FoldingRangeKind;

namespace KSPCompiler.Applications.LSServer.LanguageServerFramework.Folding.Extensions;

public static class FoldingItemExtension
{
    public static FrameworkFoldingRange As( this FoldingItem self )
    {
        return new FrameworkFoldingRange
        {
            StartLine      = (uint)( self.Position.BeginLine.Value - 1 ), // 1-based to 0-based
            StartCharacter = (uint)self.Position.BeginColumn.Value,
            EndLine        = (uint)( self.Position.EndLine.Value - 1 ), // 1-based to 0-based
            EndCharacter   = (uint)self.Position.EndColumn.Value,
            Kind           = self.Kind.As()
        };
    }

    public static List<FrameworkFoldingRange> As( this IEnumerable<FoldingItem> self )
    {
        var result = new List<FrameworkFoldingRange>();

        foreach( var x in self )
        {
            result.Add( x.As() );
        }

        return result;
    }

    public static FrameworkFoldingRangeKind? As( this FoldingRangeKind self )
    {
        return self switch
        {
            FoldingRangeKind.Comment => FrameworkFoldingRangeKind.Comment,
            FoldingRangeKind.Region  => FrameworkFoldingRangeKind.Region,
            FoldingRangeKind.Imports => FrameworkFoldingRangeKind.Imports,
            _                        => null
        };
    }
}
