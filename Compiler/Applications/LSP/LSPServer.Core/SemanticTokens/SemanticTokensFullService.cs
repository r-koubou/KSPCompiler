using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.LSPServer.Core.Compilations;

using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace KSPCompiler.LSPServer.Core.SemanticTokens;

public sealed class SemanticTokensFullService( CompilerCacheService compilerCacheService )
{
    public static readonly ImmutableArray<SemanticTokenType> LegendTokenTypes
        = SemanticTokenType.Defaults.ToImmutableArray();

    public static readonly ImmutableArray<SemanticTokenModifier> LegendTokenModifiers
        = SemanticTokenModifier.Defaults.ToImmutableArray();

    private CompilerCacheService CompilerCacheService { get; } = compilerCacheService;

    public async Task<OmniSharp.Extensions.LanguageServer.Protocol.Models.SemanticTokens?> HandleAsync( SemanticTokensParams request, CancellationToken cancellationToken )
    {
        var cache = CompilerCacheService.GetCache( request.TextDocument.Uri );
        var finder = new AstSemanticTokenFinder(
            cache.SymbolTable,
            LegendTokenTypes,
            LegendTokenModifiers
        );

        await Task.CompletedTask;

        return new OmniSharp.Extensions.LanguageServer.Protocol.Models.SemanticTokens
        {
            Data = finder.Find( cache.Ast )
        };
    }
}
