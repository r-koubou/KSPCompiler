using System.Threading;
using System.Threading.Tasks;

using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Client.ClientCapabilities;
using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Server;
using EmmyLua.LanguageServer.Framework.Protocol.Message.Definition;
using EmmyLua.LanguageServer.Framework.Server.Handler;

using KSPCompiler.Applications.LSServer.LanguageServerFramework.Extensions;
using KSPCompiler.Interactors.LanguageServer.Compilation;
using KSPCompiler.Interactors.LanguageServer.Definition;
using KSPCompiler.UseCases.LanguageServer.Definition;

namespace KSPCompiler.Applications.LSServer.LanguageServerFramework.Definition;

public class DefinitionHandler(
    CompilationCacheManager compilationCacheManager
) : DefinitionHandlerBase
{
    private readonly CompilationCacheManager compilationCacheManager = compilationCacheManager;
    private readonly DefinitionInteractor service = new();

    protected override async Task<DefinitionResponse?> Handle( DefinitionParams request, CancellationToken cancellationToken )
    {
        var scriptLocation = request.TextDocument.Uri.AsScriptLocation();
        var position = request.Position.As();

        if( !compilationCacheManager.ContainsCache( scriptLocation ) )
        {
            return null;
        }

        var input = new DefinitionInputPort(
            new DefinitionInputPortDetail( compilationCacheManager, scriptLocation, position )
        );
        var result = await service.ExecuteAsync( input, cancellationToken );

        return new DefinitionResponse( result.OutputData.As() );
    }

    public override void RegisterCapability( ServerCapabilities serverCapabilities, ClientCapabilities clientCapabilities )
    {
        serverCapabilities.DefinitionProvider = true;
    }
}
