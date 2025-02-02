using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.LSPServer.Core.Compilations;

using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace KSPCompiler.LSPServer.Core.SemanticTokens;

public sealed class SemanticTokensFullService( CompilerCacheService compilerCacheService )
{
    public static readonly ImmutableArray<SemanticTokenType> LegendTokenTypes = ImmutableArray.Create(
        SemanticTokenType.Function,
        SemanticTokenType.Method,
        SemanticTokenType.Event,
        SemanticTokenType.Keyword,
        SemanticTokenType.String,
        SemanticTokenType.Variable
    );

    public static readonly ImmutableArray<SemanticTokenModifier> LegendTokenModifiers = ImmutableArray.Create(
        SemanticTokenModifier.Readonly,
        SemanticTokenModifier.DefaultLibrary,
        SemanticTokenModifier.Declaration
    );

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
