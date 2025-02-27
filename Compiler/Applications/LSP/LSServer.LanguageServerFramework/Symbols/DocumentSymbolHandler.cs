using System.Threading;
using System.Threading.Tasks;

using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Client.ClientCapabilities;
using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Server;
using EmmyLua.LanguageServer.Framework.Protocol.Message.DocumentSymbol;
using EmmyLua.LanguageServer.Framework.Server.Handler;

using KSPCompiler.Applications.LSServer.LanguageServerFramework.Extensions;
using KSPCompiler.Applications.LSServer.LanguageServerFramework.Symbols.Extensions;
using KSPCompiler.Interactors.LanguageServer.Compilation;
using KSPCompiler.Interactors.LanguageServer.SignatureHelp;
using KSPCompiler.UseCases.LanguageServer.Symbol;

namespace KSPCompiler.Applications.LSServer.LanguageServerFramework.Symbols;

public sealed class DocumentSymbolHandler(
    CompilationCacheManager compilationCacheManager
) : DocumentSymbolHandlerBase
{
    private readonly CompilationCacheManager compilationCacheManager = compilationCacheManager;
    private readonly DocumentSymbolInteractor interactor = new();

    protected override async Task<DocumentSymbolResponse> Handle( DocumentSymbolParams request, CancellationToken token )
    {
        var scriptLocation = request.TextDocument.Uri.AsScriptLocation();

        var input = new DocumentSymbolInputPort(
            new DocumentSymbolInputPortDetail( compilationCacheManager, scriptLocation )
        );

        var result = await interactor.ExecuteAsync( input, token );

        return new DocumentSymbolResponse( result.OutputData.As() );
    }

    public override void RegisterCapability( ServerCapabilities serverCapabilities, ClientCapabilities clientCapabilities )
    {
        serverCapabilities.DocumentSymbolProvider = true;
    }
}
