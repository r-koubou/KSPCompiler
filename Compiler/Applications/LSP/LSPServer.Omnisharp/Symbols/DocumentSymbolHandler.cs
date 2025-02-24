using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Applications.LSPServer.Omnisharp.Compilations;

using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace KSPCompiler.Applications.LSPServer.Omnisharp.Symbols;

public class DocumentSymbolHandler(
    CompilerCacheService compilerCacheService,
    SymbolInformationService symbolInformationService ) : IDocumentSymbolHandler
{
    private CompilerCacheService CompilerCacheService { get; } = compilerCacheService;
    private SymbolInformationService SymbolInformationService { get; } = symbolInformationService;

    public async Task<SymbolInformationOrDocumentSymbolContainer?> Handle( DocumentSymbolParams request, CancellationToken cancellationToken )
    {
        return await SymbolInformationService.HandleAsync( CompilerCacheService, request, cancellationToken );
    }

    public DocumentSymbolRegistrationOptions GetRegistrationOptions( DocumentSymbolCapability capability, ClientCapabilities clientCapabilities )
        => new()
        {
            DocumentSelector = ConstantValues.TextDocumentSelector
        };
}
