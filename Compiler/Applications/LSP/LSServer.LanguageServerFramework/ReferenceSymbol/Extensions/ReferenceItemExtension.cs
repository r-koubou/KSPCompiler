using System.Collections.Generic;

using EmmyLua.LanguageServer.Framework.Protocol.Model;

using KSPCompiler.Applications.LSPServer.CoreNew.Reference;
using KSPCompiler.Applications.LSServer.LanguageServerFramework.Extensions;

namespace KSPCompiler.Applications.LSServer.LanguageServerFramework.ReferenceSymbol.Extensions;

public static class ReferenceItemExtension
{
    public static Location As( this ReferenceItem self )
    {
        return new Location
        {
            Uri   = self.Location.AsDocumentUri(),
            Range = self.Range.AsRange()
        };
    }

    public static List<Location> As( this IEnumerable<ReferenceItem> self )
    {
        var result = new List<Location>();

        foreach( var x in self )
        {
            result.Add( x.As() );
        }

        return result;
    }
}
