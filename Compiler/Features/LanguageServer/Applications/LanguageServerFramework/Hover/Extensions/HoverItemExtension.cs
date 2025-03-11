using EmmyLua.LanguageServer.Framework.Protocol.Message.Hover;

using KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.Extensions;
using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Hover;

namespace KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.Hover.Extensions;

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
