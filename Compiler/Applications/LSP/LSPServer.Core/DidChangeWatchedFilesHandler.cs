using System.Threading;
using System.Threading.Tasks;

using MediatR;

using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Workspace;

namespace KSPCompiler.LSPServer.Core;

internal class DidChangeWatchedFilesHandler : IDidChangeWatchedFilesHandler
{
    public DidChangeWatchedFilesRegistrationOptions GetRegistrationOptions() => new();

    public Task<Unit> Handle(DidChangeWatchedFilesParams request, CancellationToken cancellationToken)
    {
        return Unit.Task;
    }

    public DidChangeWatchedFilesRegistrationOptions GetRegistrationOptions(DidChangeWatchedFilesCapability capability, ClientCapabilities clientCapabilities) => new ();
}
