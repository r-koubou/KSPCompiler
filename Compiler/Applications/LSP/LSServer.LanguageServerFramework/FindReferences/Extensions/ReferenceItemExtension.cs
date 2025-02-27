using System.Collections.Generic;

using EmmyLua.LanguageServer.Framework.Protocol.Model;

using KSPCompiler.Applications.LSServer.LanguageServerFramework.Extensions;
using KSPCompiler.UseCases.LanguageServer.FindReferences;

namespace KSPCompiler.Applications.LSServer.LanguageServerFramework.FindReferences.Extensions;

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
