using System.Threading;
using System.Threading.Tasks;

using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Client.ClientCapabilities;
using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Server;
using EmmyLua.LanguageServer.Framework.Protocol.Message.Hover;
using EmmyLua.LanguageServer.Framework.Server.Handler;

using KSPCompiler.Applications.LSPServer.Core.Compilation;
using KSPCompiler.Applications.LSPServer.Core.Hover;
using KSPCompiler.Applications.LSServer.LanguageServerFramework.Extensions;
using KSPCompiler.Applications.LSServer.LanguageServerFramework.Hover.Extensions;

namespace KSPCompiler.Applications.LSServer.LanguageServerFramework.Hover;

public sealed class HoverHandler( CompilationCacheManager compilationCacheManager ) : HoverHandlerBase
{
    private readonly CompilationCacheManager compilationCacheManager = compilationCacheManager;
    private readonly HoverHandlingService service = new();

    protected override async Task<HoverResponse?> Handle( HoverParams request, CancellationToken token )
    {
        var scriptLocation = request.TextDocument.Uri.AsScriptLocation();
        var position = request.Position.As();
        var result = await service.HandleAsync( compilationCacheManager, scriptLocation, position, token );

        return result?.As();
    }

    public override void RegisterCapability( ServerCapabilities serverCapabilities, ClientCapabilities clientCapabilities )
    {
        serverCapabilities.HoverProvider = true;
    }
}
