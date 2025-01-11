using System;
using System.Threading;
using System.Threading.Tasks;

using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace KSPCompiler.LSPServer.Core;

public class CompletionResolveHandler : ICompletionResolveHandler
{
    private CompletionCapability? capability;

    public Guid Id { get; } = Guid.NewGuid();

    public async Task<CompletionItem> Handle( CompletionItem request, CancellationToken cancellationToken )
    {
        await Task.CompletedTask;

        return request;
    }

    public void SetCapability( CompletionCapability newCapability, ClientCapabilities clientCapabilities )
    {
        capability = newCapability;
    }
}
