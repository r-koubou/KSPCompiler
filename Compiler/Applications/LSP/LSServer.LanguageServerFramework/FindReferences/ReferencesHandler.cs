using System.Threading;
using System.Threading.Tasks;

using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Client.ClientCapabilities;
using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Server;
using EmmyLua.LanguageServer.Framework.Protocol.Message.Reference;
using EmmyLua.LanguageServer.Framework.Server.Handler;

using KSPCompiler.Applications.LSPServer.CoreNew.Compilation;
using KSPCompiler.Applications.LSPServer.CoreNew.FindReferences;
using KSPCompiler.Applications.LSServer.LanguageServerFramework.Extensions;
using KSPCompiler.Applications.LSServer.LanguageServerFramework.FindReferences.Extensions;

namespace KSPCompiler.Applications.LSServer.LanguageServerFramework.FindReferences;

public class ReferencesHandler(
    CompilationCacheManager compilationCacheManager
    )
: ReferenceHandlerBase
{
    private readonly CompilationCacheManager compilationCacheManager = compilationCacheManager;
    private readonly ReferenceHandlingService referenceHandlingService = new ();

    protected override async Task<ReferenceResponse?> Handle( ReferenceParams request, CancellationToken cancellationToken )
    {
        var scriptLocation = request.TextDocument.Uri.AsScriptLocation();
        var position = request.Position.As();

        if( !compilationCacheManager.ContainsCache( scriptLocation ) )
        {
            return null;
        }

        var result = await referenceHandlingService.HandleAsync( compilationCacheManager, scriptLocation, position, cancellationToken );

        return new ReferenceResponse( result.As() );

    }

    public override void RegisterCapability( ServerCapabilities serverCapabilities, ClientCapabilities clientCapabilities )
    {
        serverCapabilities.ReferencesProvider = true;
    }
}
