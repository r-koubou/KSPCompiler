using System.Threading;
using System.Threading.Tasks;

using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace KSPCompiler.LSPServer.Core.Renames;

public class RenameHandler( RenameService renameService ) : IRenameHandler, IPrepareRenameHandler
{
    private RenameService RenameService { get; } = renameService;

    public async Task<WorkspaceEdit?> Handle( RenameParams request, CancellationToken cancellationToken )
        => await RenameService.HandleAsync( request, cancellationToken );

    public RenameRegistrationOptions GetRegistrationOptions( RenameCapability capability, ClientCapabilities clientCapabilities )
        => new()
        {
            DocumentSelector = ConstantValues.TextDocumentSelector
        };

    public async Task<RangeOrPlaceholderRange?> Handle( PrepareRenameParams request, CancellationToken cancellationToken )
        => await RenameService.HandleAsync( request, cancellationToken );
}
