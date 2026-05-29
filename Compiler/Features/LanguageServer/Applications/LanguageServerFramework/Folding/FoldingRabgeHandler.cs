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

        var input = new FoldingRangeInput(
            compilationCacheManager,
            scriptLocation
        );

        var result = await interactor.ExecuteAsync( input, token );

        if( result.IsFailure )
        {
            return new FoldingRangeResponse( [] );
        }

        var output = result.Unwrap();

        return output.Ranges.Count == 0
            ? new FoldingRangeResponse( [] )
            : new FoldingRangeResponse( output.Ranges.As() );
    }

    public override void RegisterCapability( ServerCapabilities serverCapabilities, ClientCapabilities clientCapabilities )
    {
        serverCapabilities.FoldingRangeProvider = true;
    }
}
