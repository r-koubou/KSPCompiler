using System.Threading;
using System.Threading.Tasks;

using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Client.ClientCapabilities;
using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Server;
using EmmyLua.LanguageServer.Framework.Protocol.Message.Hover;
using EmmyLua.LanguageServer.Framework.Server.Handler;

using KSPCompiler.Applications.LSServer.LanguageServerFramework.Extensions;
using KSPCompiler.Applications.LSServer.LanguageServerFramework.Hover.Extensions;
using KSPCompiler.Interactors.LanguageServer.Compilation;
using KSPCompiler.Interactors.LanguageServer.Hover;
using KSPCompiler.UseCases.LanguageServer.Hover;

namespace KSPCompiler.Applications.LSServer.LanguageServerFramework.Hover;

public sealed class HoverHandler( CompilationCacheManager compilationCacheManager ) : HoverHandlerBase
{
    private readonly CompilationCacheManager compilationCacheManager = compilationCacheManager;
    private readonly HoverInteractor interactor = new();

    protected override async Task<HoverResponse?> Handle( HoverParams request, CancellationToken token )
    {
        var scriptLocation = request.TextDocument.Uri.AsScriptLocation();
        var position = request.Position.As();

        var input = new HoverInputPort(
            new HoverInputPortDetail( compilationCacheManager, scriptLocation, position )
        );
        var result = await interactor.ExecuteAsync( input, token );

        return result.OutputData?.As();
    }

    public override void RegisterCapability( ServerCapabilities serverCapabilities, ClientCapabilities clientCapabilities )
    {
        serverCapabilities.HoverProvider = true;
    }
}
