using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.LSPServer.Core.Compilations;

using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace KSPCompiler.LSPServer.Core.SemanticTokensRanges;

public sealed class SemanticTokensRangeService( CompilerCacheService compilerCacheService )
{
    public static readonly ImmutableArray<SemanticTokenType> LegendTokenTypes = ImmutableArray.Create(
        SemanticTokenType.Function,
        SemanticTokenType.String,
        SemanticTokenType.Variable
    );

    public static readonly ImmutableArray<SemanticTokenModifier> LegendTokenModifiers = ImmutableArray.Create(
        SemanticTokenModifier.Readonly,
        SemanticTokenModifier.DefaultLibrary,
        SemanticTokenModifier.Declaration
    );

    private CompilerCacheService CompilerCacheService { get; } = compilerCacheService;

    public async Task<SemanticTokens?> HandleAsync( SemanticTokensRangeParams request, CancellationToken cancellationToken )
    {
        var cache = CompilerCacheService.GetCache( request.TextDocument.Uri );
        var finder = new AstSemanticTokenFinder(
            cache.SymbolTable,
            LegendTokenTypes,
            LegendTokenModifiers
        );

        await Task.CompletedTask;

        return new SemanticTokens
        {
            Data = finder.Find( cache.Ast )
        };
    }
}
