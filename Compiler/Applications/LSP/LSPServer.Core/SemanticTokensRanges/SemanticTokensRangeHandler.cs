using System.Threading;
using System.Threading.Tasks;

using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace KSPCompiler.LSPServer.Core.SemanticTokensRanges;

public class SemanticTokensRangeHandler( SemanticTokensRangeService semanticTokensRangeService ) : ISemanticTokensRangeHandler
{
    private SemanticTokensRangeService SemanticTokensRangeService { get; } = semanticTokensRangeService;

    public async Task<SemanticTokens?> Handle( SemanticTokensRangeParams request, CancellationToken cancellationToken )
        => await SemanticTokensRangeService.HandleAsync( request, cancellationToken );

    public SemanticTokensRegistrationOptions GetRegistrationOptions( SemanticTokensCapability capability, ClientCapabilities clientCapabilities )
        => new()
        {
            DocumentSelector = ConstantValues.TextDocumentSelector,
            Full = new SemanticTokensCapabilityRequestFull
            {
                Delta = false
            },
        };
}
