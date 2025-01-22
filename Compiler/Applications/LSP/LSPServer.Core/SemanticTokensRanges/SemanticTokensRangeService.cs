using System.Threading;
using System.Threading.Tasks;

using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace KSPCompiler.LSPServer.Core.SemanticTokensRanges;

public sealed class SemanticTokensRangeService
{
    public Task<SemanticTokens?> HandleAsync( SemanticTokensRangeParams request, CancellationToken cancellationToken )
        => throw new System.NotImplementedException();
}
