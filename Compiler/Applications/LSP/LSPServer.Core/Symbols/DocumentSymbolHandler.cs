using System.Threading;
using System.Threading.Tasks;

using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace KSPCompiler.LSPServer.Core.Symbols;

public class DocumentSymbolHandler(
    CompilerCache compilerCache,
    SymbolInformationService symbolInformationService ) : IDocumentSymbolHandler
{
    private CompilerCache CompilerCache { get; } = compilerCache;
    private SymbolInformationService SymbolInformationService { get; } = symbolInformationService;

    public async Task<SymbolInformationOrDocumentSymbolContainer?> Handle( DocumentSymbolParams request, CancellationToken cancellationToken )
    {
        return await SymbolInformationService.HandleAsync( CompilerCache, request, cancellationToken );
    }

    public DocumentSymbolRegistrationOptions GetRegistrationOptions( DocumentSymbolCapability capability, ClientCapabilities clientCapabilities )
        => new()
        {
            DocumentSelector = ConstantValues.TextDocumentSelector
        };
}
