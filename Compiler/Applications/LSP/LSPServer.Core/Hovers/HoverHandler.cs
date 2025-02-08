using System.Threading;
using System.Threading.Tasks;

using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace KSPCompiler.Applications.LSPServer.Core.Hovers;

public class HoverHandler( HoverService hoverService ) : IHoverHandler
{
    private HoverService HoverService { get; } = hoverService;

    public async Task<Hover?> Handle( HoverParams request, CancellationToken cancellationToken )
        => await HoverService.HandleAsync( request, cancellationToken );

    public HoverRegistrationOptions GetRegistrationOptions( HoverCapability capability, ClientCapabilities clientCapabilities )
        => new()
        {
            DocumentSelector = ConstantValues.TextDocumentSelector
        };
}
