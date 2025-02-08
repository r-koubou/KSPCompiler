using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace KSPCompiler.Applications.LSPServer.Core.Hovers.Extensions;

public static class StringExtension
{
    public static Hover AsHover( this string self )
        => new()
        {
            Contents = new MarkedStringsOrMarkupContent(
                new MarkedString( self )
            )
        };
}
