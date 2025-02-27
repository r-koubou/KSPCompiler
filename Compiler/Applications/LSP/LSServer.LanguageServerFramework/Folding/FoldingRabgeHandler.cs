using System.Threading;
using System.Threading.Tasks;

using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Client.ClientCapabilities;
using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Server;
using EmmyLua.LanguageServer.Framework.Protocol.Message.FoldingRange;
using EmmyLua.LanguageServer.Framework.Server.Handler;

using KSPCompiler.Applications.LSServer.LanguageServerFramework.Extensions;
using KSPCompiler.Applications.LSServer.LanguageServerFramework.Folding.Extensions;
using KSPCompiler.Interactors.LanguageServer.Folding;
using KSPCompiler.UseCases.LanguageServer.Compilation;
using KSPCompiler.UseCases.LanguageServer.Folding;

namespace KSPCompiler.Applications.LSServer.LanguageServerFramework.Folding;

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
            new FoldingRangeInputPortDetail( compilationCacheManager, scriptLocation )
        );
        var result = await interactor.ExecuteAsync( input, token );

        return new FoldingRangeResponse( result.OutputData.As() );
    }

    public override void RegisterCapability( ServerCapabilities serverCapabilities, ClientCapabilities clientCapabilities )
    {
        serverCapabilities.FoldingRangeProvider = true;
    }
}
