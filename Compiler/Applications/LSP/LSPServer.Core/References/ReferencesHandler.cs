using System.Threading;
using System.Threading.Tasks;

using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace KSPCompiler.Applications.LSPServer.Core.References;

public class ReferencesHandler( ReferencesService referencesService ) : IReferencesHandler
{
    private ReferencesService ReferencesService { get; } = referencesService;

    public async Task<LocationContainer?> Handle( ReferenceParams request, CancellationToken cancellationToken )
        => await ReferencesService.HandleAsync( request, cancellationToken );

    public ReferenceRegistrationOptions GetRegistrationOptions( ReferenceCapability capability, ClientCapabilities clientCapabilities )
        => new()
        {
            DocumentSelector = ConstantValues.TextDocumentSelector
        };
}
