using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Folding;
using KSPCompiler.Features.LanguageServer.UseCase.Ast;
using KSPCompiler.Features.LanguageServer.UseCase.Folding.Extensions;

namespace KSPCompiler.Features.LanguageServer.UseCase.Folding;

public sealed class FoldingRangeInteractor : IFoldingRangeUseCase
{
    public async Task<FoldingRangeOutputPort> ExecuteAsync(
        FoldingRangeInputPort parameter,
        CancellationToken cancellationToken = default )
    {
        try
        {
            var compilationCacheManager = parameter.Input.Cache;
            var scriptLocation = parameter.Input.Location;

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

            return new FoldingRangeOutputPort( items, true );
        }
        catch( Exception e )
        {
            return new FoldingRangeOutputPort( [], false, e );
        }
    }
}
