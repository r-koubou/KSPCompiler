using System.Threading;
using System.Threading.Tasks;

using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Client.ClientCapabilities;
using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Server;
using EmmyLua.LanguageServer.Framework.Protocol.Message.DocumentSymbol;
using EmmyLua.LanguageServer.Framework.Server.Handler;

using KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.Extensions;
using KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.Symbols.Extensions;
using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Compilation;
using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Symbol;
using KSPCompiler.Features.LanguageServer.UseCase.SignatureHelp;

namespace KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.Symbols;

public sealed class DocumentSymbolHandler(
    ICompilationCacheManager compilationCacheManager
) : DocumentSymbolHandlerBase
{
    private readonly ICompilationCacheManager compilationCacheManager = compilationCacheManager;
    private readonly DocumentSymbolInteractor interactor = new();

    protected override async Task<DocumentSymbolResponse> Handle( DocumentSymbolParams request, CancellationToken token )
    {
        var scriptLocation = request.TextDocument.Uri.AsScriptLocation();

        var input = new DocumentSymbolInputPort(
            new DocumentSymbolInputPortDetail(
                compilationCacheManager,
                scriptLocation
            )
        );

        var output = await interactor.ExecuteAsync( input, token );

        if( output.OutputData.Count == 0 )
        {
            return new DocumentSymbolResponse( [] );
        }

        return new DocumentSymbolResponse( output.OutputData.As() );
    }

    public override void RegisterCapability( ServerCapabilities serverCapabilities, ClientCapabilities clientCapabilities )
    {
        serverCapabilities.DocumentSymbolProvider = true;
    }
}
