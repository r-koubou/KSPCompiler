using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Interactors.LanguageServer.Ast;
using KSPCompiler.Interactors.LanguageServer.Folding.Extensions;
using KSPCompiler.UseCases.LanguageServer.Folding;

namespace KSPCompiler.Interactors.LanguageServer.Folding;

public sealed class FoldingRangeInteractor : IFoldingRangeUseCase
{
    public async Task<FoldingOutputPort> ExecuteAsync(
        FoldingInputPort parameter,
        CancellationToken cancellationToken = default )
    {
        try
        {
            var compilationCacheManager = parameter.Data.Cache;
            var scriptLocation = parameter.Data.Location;

            var cache = compilationCacheManager.GetCache( scriptLocation );
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

            return new FoldingOutputPort( items, true );
        }
        catch( Exception e )
        {
            return new FoldingOutputPort( [], false, e );
        }
    }
}
