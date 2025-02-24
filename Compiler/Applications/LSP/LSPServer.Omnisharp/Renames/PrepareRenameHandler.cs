using System.Threading;
using System.Threading.Tasks;

using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace KSPCompiler.Applications.LSPServer.Omnisharp.Renames;

public class PrepareRenameHandler( RenameService renameService ) : IPrepareRenameHandler
{
    private RenameService RenameService { get; } = renameService;

    public async Task<RangeOrPlaceholderRange?> Handle( PrepareRenameParams request, CancellationToken cancellationToken )
        => await RenameService.HandleAsync( request, cancellationToken );

    public RenameRegistrationOptions GetRegistrationOptions( RenameCapability capability, ClientCapabilities clientCapabilities )
        => new()
        {
            DocumentSelector = ConstantValues.TextDocumentSelector
        };
}
