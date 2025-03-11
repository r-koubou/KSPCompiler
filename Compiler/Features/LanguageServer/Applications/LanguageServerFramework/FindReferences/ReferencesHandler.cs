using System.Threading;
using System.Threading.Tasks;

using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Client.ClientCapabilities;
using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Server;
using EmmyLua.LanguageServer.Framework.Protocol.Message.Reference;
using EmmyLua.LanguageServer.Framework.Server.Handler;

using KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.Extensions;
using KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.FindReferences.Extensions;
using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Compilation;
using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.FindReferences;
using KSPCompiler.Features.LanguageServer.UseCase.FindReferences;

namespace KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.FindReferences;

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
            new FindReferenceInputPortDetail(
                compilationCacheManager,
                scriptLocation,
                position
            )
        );

        var output = await interactor.ExecuteAsync( input, cancellationToken );

        if( !output.Result || output.OutputData.Count == 0 )
        {
            return null;
        }

        return new ReferenceResponse( output.OutputData.As() );

    }

    public override void RegisterCapability( ServerCapabilities serverCapabilities, ClientCapabilities clientCapabilities )
    {
        serverCapabilities.ReferencesProvider = true;
    }
}
