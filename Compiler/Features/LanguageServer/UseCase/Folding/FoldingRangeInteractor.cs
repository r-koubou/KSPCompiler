using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.LanguageServer.UseCase.Abstractions;
using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Folding;
using KSPCompiler.Features.LanguageServer.UseCase.Ast;
using KSPCompiler.Features.LanguageServer.UseCase.Folding.Extensions;
using KSPCompiler.Shared;

namespace KSPCompiler.Features.LanguageServer.UseCase.Folding;

public sealed class FoldingRangeInteractor : IFoldingRangeUseCase
{
    public async Task<Result<FoldingRangeOutput, LanguageServerFailureReason>> ExecuteAsync(
        FoldingRangeInput input,
        CancellationToken cancellationToken = default )
    {
        try
        {
            var compilationCacheManager = input.Cache;
            var scriptLocation = input.Location;

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

            return Result<FoldingRangeOutput, LanguageServerFailureReason>.Success( new FoldingRangeOutput( items ) );
        }
        catch( Exception e )
        {
            return Result<FoldingRangeOutput, LanguageServerFailureReason>.Failure( LanguageServerFailureReason.Other, e );
        }
    }
}
