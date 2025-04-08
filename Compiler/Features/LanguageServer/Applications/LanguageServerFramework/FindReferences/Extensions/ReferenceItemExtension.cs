using System.Collections.Generic;

using EmmyLua.LanguageServer.Framework.Protocol.Model;

using KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.Extensions;
using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.FindReferences;

namespace KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.FindReferences.Extensions;

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
