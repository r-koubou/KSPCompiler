using System.Threading;
using System.Threading.Tasks;

using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Client.ClientCapabilities;
using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Server;
using EmmyLua.LanguageServer.Framework.Protocol.Message.Definition;
using EmmyLua.LanguageServer.Framework.Server.Handler;

using KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.Extensions;
using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Compilation;
using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Definition;
using KSPCompiler.Features.LanguageServer.UseCase.Definition;

namespace KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.Definition;

public class DefinitionHandler(
    ICompilationCacheManager compilationCacheManager
) : DefinitionHandlerBase
{
    private readonly ICompilationCacheManager compilationCacheManager = compilationCacheManager;
    private readonly DefinitionInteractor interactor = new();

    protected override async Task<DefinitionResponse?> Handle( DefinitionParams request, CancellationToken cancellationToken )
    {
        var scriptLocation = request.TextDocument.Uri.AsScriptLocation();
        var position = request.Position.As();

        if( !compilationCacheManager.ContainsCache( scriptLocation ) )
        {
            return null;
        }

        var input = new DefinitionInputPort(
            new DefinitionInputPortDetail(
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

        return new DefinitionResponse( output.OutputData.As() );
    }

    public override void RegisterCapability( ServerCapabilities serverCapabilities, ClientCapabilities clientCapabilities )
    {
        serverCapabilities.DefinitionProvider = true;
    }
}
