using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Applications.LSPServer.CoreNew.Ast;
using KSPCompiler.Applications.LSPServer.CoreNew.Compilation;
using KSPCompiler.Applications.LSPServer.CoreNew.Folding.Extensions;

namespace KSPCompiler.Applications.LSPServer.CoreNew.Folding;

public sealed class FoldingRangeHandlingService
{
    public async Task<List<FoldingItem>> HandleAsync(
        CompilationCacheManager compilerCacheService,
        ScriptLocation scriptLocation,
        CancellationToken _ )
    {
        var cache = compilerCacheService.GetCache( scriptLocation );
        var symbols = cache.SymbolTable;
        var items = new List<FoldingItem>();

        // ユーザー実装コールバック
        foreach( var callback in symbols.UserCallbacks.ToList() )
        {
            items.Add( callback.AsFoldingRange() );
        }

        // ユーザー実装関数
        foreach( var function in symbols.UserFunctions )
        {
            items.Add( function.AsFoldingRange() );
        }

        // ステートメント
        var finder = new FoldingSupportedNodeAppearanceFinder();
        foreach( var x in finder.Find( cache.Ast ) )
        {
            items.Add( x.AsFoldingRange() );
        }

        await Task.CompletedTask;

        return items;
    }
}
