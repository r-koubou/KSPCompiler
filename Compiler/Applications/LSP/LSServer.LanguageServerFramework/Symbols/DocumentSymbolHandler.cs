using System.Threading;
using System.Threading.Tasks;

using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Client.ClientCapabilities;
using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Server;
using EmmyLua.LanguageServer.Framework.Protocol.Message.DocumentSymbol;
using EmmyLua.LanguageServer.Framework.Server.Handler;

using KSPCompiler.Applications.LSPServer.CoreNew.Compilation;
using KSPCompiler.Applications.LSPServer.CoreNew.Symbol;
using KSPCompiler.Applications.LSServer.LanguageServerFramework.Extensions;
using KSPCompiler.Applications.LSServer.LanguageServerFramework.Symbols.Extensions;

namespace KSPCompiler.Applications.LSServer.LanguageServerFramework.Symbols;

public sealed class DocumentSymbolHandler(
    CompilationCacheManager compilationCacheManager
) : DocumentSymbolHandlerBase
{
    private readonly CompilationCacheManager compilationCacheManager = compilationCacheManager;
    private readonly SymbolInformationHandlingService service = new();

    protected override async Task<DocumentSymbolResponse> Handle( DocumentSymbolParams request, CancellationToken token )
    {
        var scriptLocation = request.TextDocument.Uri.AsScriptLocation();
        var result = await service.HandleAsync( compilationCacheManager, scriptLocation, token );

        return new DocumentSymbolResponse( result.As() );
    }

    public override void RegisterCapability( ServerCapabilities serverCapabilities, ClientCapabilities clientCapabilities )
    {
        serverCapabilities.DocumentSymbolProvider = true;
    }
}
