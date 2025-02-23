using System.Threading;
using System.Threading.Tasks;

using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Client.ClientCapabilities;
using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Server;
using EmmyLua.LanguageServer.Framework.Protocol.Message.Definition;
using EmmyLua.LanguageServer.Framework.Server.Handler;

using KSPCompiler.Applications.LSPServer.CoreNew.Compilation;
using KSPCompiler.Applications.LSPServer.CoreNew.Definition;
using KSPCompiler.Applications.LSServer.LanguageServerFramework.Extensions;

namespace KSPCompiler.Applications.LSServer.LanguageServerFramework.Definition;

public class DefinitionHandler(
    CompilationCacheManager compilationCacheManager
) : DefinitionHandlerBase
{
    private readonly CompilationCacheManager compilationCacheManager = compilationCacheManager;
    private readonly DefinitionHandlingService service = new();

    protected override async Task<DefinitionResponse?> Handle( DefinitionParams request, CancellationToken cancellationToken )
    {
        var scriptLocation = request.TextDocument.Uri.AsScriptLocation();
        var position = request.Position.As();

        if( !compilationCacheManager.ContainsCache( scriptLocation ) )
        {
            return null;
        }

        var result = await service.HandleAsync( compilationCacheManager, scriptLocation, position, cancellationToken );

        return new DefinitionResponse( result.As() );
    }

    public override void RegisterCapability( ServerCapabilities serverCapabilities, ClientCapabilities clientCapabilities )
    {
        serverCapabilities.DefinitionProvider = true;
    }
}
