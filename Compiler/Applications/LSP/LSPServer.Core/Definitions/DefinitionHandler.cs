using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.LSPServer.Core.Compilations;
using KSPCompiler.LSPServer.Core.Extensions;

using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace KSPCompiler.LSPServer.Core.Definitions;

public class DefinitionHandler( DefinitionService definitionService ) : IDefinitionHandler
{
    private DefinitionService DefinitionService { get; } = definitionService;

    #region IDefinitionHandler
    public async Task<LocationOrLocationLinks?> Handle( DefinitionParams request, CancellationToken cancellationToken )
        => await DefinitionService.HandleAsync( request, cancellationToken );

    public DefinitionRegistrationOptions GetRegistrationOptions( DefinitionCapability capability, ClientCapabilities clientCapabilities )
        => new()
        {
            DocumentSelector = ConstantValues.TextDocumentSelector
        };
    #endregion ~IDefinitionHandler
}
