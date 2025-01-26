using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.LSPServer.Core.Ast;
using KSPCompiler.LSPServer.Core.Compilations;
using KSPCompiler.LSPServer.Core.Foldings.Extensions;

using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace KSPCompiler.LSPServer.Core.Foldings;

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
