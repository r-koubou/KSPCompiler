using System.Threading;
using System.Threading.Tasks;

using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace KSPCompiler.Applications.LSPServer.Core.DocumentHighlights;

public class DocumentHighlightHandler( DocumentHighlightService documentHighlightService ) : IDocumentHighlightHandler
{
    private DocumentHighlightService DocumentHighlightService { get; } = documentHighlightService;

    public async Task<DocumentHighlightContainer?> Handle( DocumentHighlightParams request, CancellationToken cancellationToken )
    {
        var result = await DocumentHighlightService.HandleAsync( request, cancellationToken );

        return result ?? new DocumentHighlightContainer();
    }

    public DocumentHighlightRegistrationOptions GetRegistrationOptions( DocumentHighlightCapability capability, ClientCapabilities clientCapabilities )
        => new()
        {
            DocumentSelector = ConstantValues.TextDocumentSelector
        };
}
