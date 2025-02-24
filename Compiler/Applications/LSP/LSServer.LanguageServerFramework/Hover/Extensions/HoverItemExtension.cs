using EmmyLua.LanguageServer.Framework.Protocol.Message.Hover;

using KSPCompiler.Applications.LSPServer.Core.Hover;
using KSPCompiler.Applications.LSServer.LanguageServerFramework.Extensions;

namespace KSPCompiler.Applications.LSServer.LanguageServerFramework.Hover.Extensions;

public static class HoverItemExtension
{
    public static HoverResponse As( this HoverItem self )
    {

        return new HoverResponse
        {
            Contents = self.Content.As(),
            Range = null
        };
    }
}
