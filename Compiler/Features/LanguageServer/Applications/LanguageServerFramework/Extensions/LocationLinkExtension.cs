using System.Collections.Generic;

using KSPCompiler.Features.LanguageServer.UseCase.Abstractions;

using FrameworkLocationLink = EmmyLua.LanguageServer.Framework.Protocol.Model.LocationLink;


namespace KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.Extensions;

public static class LocationLinkExtension
{
    public static FrameworkLocationLink As( this LocationLink self )
    {
        return new FrameworkLocationLink(
            OriginSelectionRange: null,
            TargetUri: self.Location.AsDocumentUri(),
            TargetRange: self.Range.AsRange(),
            TargetSelectionRange: self.Range.AsRange()
        );
    }

    public static List<FrameworkLocationLink> As( this IEnumerable<LocationLink> self )
    {
        var result = new List<FrameworkLocationLink>();

        foreach( var x in self )
        {
            result.Add( x.As() );
        }

        return result;
    }
}
