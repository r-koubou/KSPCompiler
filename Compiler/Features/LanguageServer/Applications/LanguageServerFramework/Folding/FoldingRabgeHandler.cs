using System.Threading;
using System.Threading.Tasks;

using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Client.ClientCapabilities;
using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Server;
using EmmyLua.LanguageServer.Framework.Protocol.Message.FoldingRange;
using EmmyLua.LanguageServer.Framework.Server.Handler;

using KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.Extensions;
using KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.Folding.Extensions;
using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Compilation;
using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Folding;
using KSPCompiler.Features.LanguageServer.UseCase.Folding;

namespace KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.Folding;

public sealed class FoldingRabgeHandler(
    ICompilationCacheManager compilationCacheManager
) : FoldingRangeHandlerBase
{
    private readonly ICompilationCacheManager compilationCacheManager = compilationCacheManager;
    private readonly FoldingRangeInteractor interactor = new();

    protected override async Task<FoldingRangeResponse> Handle( FoldingRangeParams request, CancellationToken token )
    {
        var scriptLocation = request.TextDocument.Uri.AsScriptLocation();

        if( !compilationCacheManager.ContainsCache( scriptLocation ) )
        {
            return new FoldingRangeResponse( [] );
        }

        var input = new FoldingRangeInputPort(
            new FoldingRangeInputPortDetail(
                compilationCacheManager,
                scriptLocation
            )
        );

        var output = await interactor.ExecuteAsync( input, token );

        if( !output.Result || output.OutputData.Count == 0 )
        {
            return new FoldingRangeResponse( [] );
        }

        return new FoldingRangeResponse( output.OutputData.As() );
    }

    public override void RegisterCapability( ServerCapabilities serverCapabilities, ClientCapabilities clientCapabilities )
    {
        serverCapabilities.FoldingRangeProvider = true;
    }
}
