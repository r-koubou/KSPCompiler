using System.Threading;
using System.Threading.Tasks;

using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace KSPCompiler.Applications.LSPServer.Core.SignatureHelps;

public class SignatureHelpHandler( SignatureHelpService signatureHelpService ) : ISignatureHelpHandler
{
    private SignatureHelpService SignatureHelpService { get; } = signatureHelpService;

    public async Task<SignatureHelp?> Handle( SignatureHelpParams request, CancellationToken cancellationToken )
        => await SignatureHelpService.HandleAsync( request, cancellationToken );

    public SignatureHelpRegistrationOptions GetRegistrationOptions( SignatureHelpCapability capability, ClientCapabilities clientCapabilities )
        => new()
        {
            DocumentSelector    = ConstantValues.TextDocumentSelector,
            TriggerCharacters   = new[] { "(", "," },
            RetriggerCharacters = new[] { "," }
        };
}
