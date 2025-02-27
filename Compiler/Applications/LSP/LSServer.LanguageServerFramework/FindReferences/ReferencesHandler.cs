using System.Threading;
using System.Threading.Tasks;

using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Client.ClientCapabilities;
using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Server;
using EmmyLua.LanguageServer.Framework.Protocol.Message.Reference;
using EmmyLua.LanguageServer.Framework.Server.Handler;

using KSPCompiler.Applications.LSServer.LanguageServerFramework.Extensions;
using KSPCompiler.Applications.LSServer.LanguageServerFramework.FindReferences.Extensions;
using KSPCompiler.Interactors.LanguageServer.FindReferences;
using KSPCompiler.UseCases.LanguageServer.Compilation;
using KSPCompiler.UseCases.LanguageServer.FindReferences;

namespace KSPCompiler.Applications.LSServer.LanguageServerFramework.FindReferences;

public sealed class ReferencesHandler(
    ICompilationCacheManager compilationCacheManager
) : ReferenceHandlerBase
{
    private readonly ICompilationCacheManager compilationCacheManager = compilationCacheManager;
    private readonly FindReferenceInteractor interactor = new ();

    protected override async Task<ReferenceResponse?> Handle( ReferenceParams request, CancellationToken cancellationToken )
    {
        var scriptLocation = request.TextDocument.Uri.AsScriptLocation();
        var position = request.Position.As();

        if( !compilationCacheManager.ContainsCache( scriptLocation ) )
        {
            return null;
        }

        var input = new FindReferenceInputPort(
            new FindReferenceInputPortDetail( compilationCacheManager, scriptLocation, position )
        );
        var result = await interactor.ExecuteAsync( input, cancellationToken );

        return new ReferenceResponse( result.OutputData.As() );

    }

    public override void RegisterCapability( ServerCapabilities serverCapabilities, ClientCapabilities clientCapabilities )
    {
        serverCapabilities.ReferencesProvider = true;
    }
}
