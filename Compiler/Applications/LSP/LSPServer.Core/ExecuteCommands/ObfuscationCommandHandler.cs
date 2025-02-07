using System.Threading;
using System.Threading.Tasks;

using MediatR;

using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Workspace;

namespace KSPCompiler.LSPServer.Core.ExecuteCommands;

public class ObfuscationCommandHandler( ExecuteCommandService executeCommandService ) : IExecuteCommandHandler<object>
{
    private ExecuteCommandService ExecuteCommandService { get; } = executeCommandService;

    public async Task<object> Handle( ExecuteCommandParams<object> request, CancellationToken cancellationToken )
        => await ExecuteCommandService.HandleAsync( request, cancellationToken );


    public ExecuteCommandRegistrationOptions GetRegistrationOptions( ExecuteCommandCapability capability, ClientCapabilities clientCapabilities )
        => ExecuteCommandService.GetRegistrationOptions( capability, clientCapabilities );
}
