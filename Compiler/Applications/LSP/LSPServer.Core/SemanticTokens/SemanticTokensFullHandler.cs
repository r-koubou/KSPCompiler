using System.Threading;
using System.Threading.Tasks;

using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace KSPCompiler.LSPServer.Core.SemanticTokens;

public class SemanticTokensFullHandler( SemanticTokensFullService semanticTokensFullService ) : ISemanticTokensFullHandler
{
    private SemanticTokensFullService SemanticTokensFullService { get; } = semanticTokensFullService;

    public async Task<OmniSharp.Extensions.LanguageServer.Protocol.Models.SemanticTokens?> Handle( SemanticTokensParams request, CancellationToken cancellationToken )
        => await SemanticTokensFullService.HandleAsync( request, cancellationToken );

    public SemanticTokensRegistrationOptions GetRegistrationOptions( SemanticTokensCapability capability, ClientCapabilities clientCapabilities )
        => new()
        {
            DocumentSelector = ConstantValues.TextDocumentSelector,
            Range            = false,
            Id               = ConstantValues.LanguageId,
            Full = new SemanticTokensCapabilityRequestFull
            {
                Delta = false
            },
            Legend = new()
            {
                TokenTypes     = SemanticTokensFullService.LegendTokenTypes,
                TokenModifiers = SemanticTokensFullService.LegendTokenModifiers
            }
        };
}
