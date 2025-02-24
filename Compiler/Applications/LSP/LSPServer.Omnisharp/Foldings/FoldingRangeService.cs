using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Applications.LSPServer.Omnisharp.Ast;
using KSPCompiler.Applications.LSPServer.Omnisharp.Compilations;
using KSPCompiler.Applications.LSPServer.Omnisharp.Foldings.Extensions;

using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace KSPCompiler.Applications.LSPServer.Omnisharp.Foldings;

public class FoldingRangeService( CompilerCacheService compilerCacheService )
{
    private CompilerCacheService CompilerCacheService { get; } = compilerCacheService;

    public async Task<Container<FoldingRange>?> HandleAsync( FoldingRangeRequestParam request, CancellationToken cancellationToken )
    {
        var cache = CompilerCacheService.GetCache( request.TextDocument.Uri );
        var symbols = cache.SymbolTable;
        var ranges = new List<FoldingRange>();

        // ユーザー実装コールバック
        foreach( var callback in symbols.UserCallbacks.ToList() )
        {
            ranges.Add( callback.AsFoldingRange() );
        }

        // ユーザー実装関数
        foreach( var function in symbols.UserFunctions )
        {
            ranges.Add( function.AsFoldingRange() );
        }

        // ステートメント
        var finder = new FoldingSupportedNodeAppearanceFinder();
        foreach( var x in finder.Find( cache.Ast ) )
        {
            ranges.Add( x.AsFoldingRange() );
        }

        await Task.CompletedTask;

        return new Container<FoldingRange>( ranges );
    }
}
