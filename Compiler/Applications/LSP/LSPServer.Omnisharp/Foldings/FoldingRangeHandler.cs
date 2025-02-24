using System.Threading;
using System.Threading.Tasks;

using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace KSPCompiler.Applications.LSPServer.Omnisharp.Foldings;

public class FoldingRangeHandler( FoldingRangeService foldingRangeService ) : IFoldingRangeHandler
{
    private FoldingRangeService FoldingRangeService { get; } = foldingRangeService;

    public async Task<Container<FoldingRange>?> Handle( FoldingRangeRequestParam request, CancellationToken cancellationToken )
        => await FoldingRangeService.HandleAsync( request, cancellationToken );

    public FoldingRangeRegistrationOptions GetRegistrationOptions( FoldingRangeCapability capability, ClientCapabilities clientCapabilities )
        => new()
        {
            DocumentSelector = ConstantValues.TextDocumentSelector
        };
}
