using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.LanguageServer.UseCase.Abstractions;
using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Definition;
using KSPCompiler.Features.LanguageServer.UseCase.Extensions;
using KSPCompiler.Shared;

namespace KSPCompiler.Features.LanguageServer.UseCase.Definition;

public sealed class DefinitionInteractor : IDefinitionHandlingUseCase
{
    public async Task<Result<DefinitionOutput, LanguageServerFailureReason>> ExecuteAsync(
        DefinitionInput input,
        CancellationToken cancellationToken = default )
    {
        try
        {
            var compilationCacheManager = input.Cache;
            var scriptLocation = input.Location;
            var position = input.Position;

            if( !compilationCacheManager.ContainsCache( scriptLocation ) )
            {
                return Result<DefinitionOutput, LanguageServerFailureReason>.Failure( LanguageServerFailureReason.DefinitionNotFound );
            }

            var cache = compilationCacheManager.GetCache( scriptLocation );
            var word = DocumentUtility.ExtractWord( cache.AllLinesText, position );
            var links = new List<LocationLink>();

            // ユーザー定義変数
            if( cache.SymbolTable.UserVariables.TrySearchDefinitionLocation( scriptLocation, word, out var result ) )
            {
                links.Add( result );
            }

            // ユーザー定義関数
            if( cache.SymbolTable.UserFunctions.TrySearchDefinitionLocation( scriptLocation, word, out result ) )
            {
                links.Add( result );
            }

            await Task.CompletedTask;

            return Result<DefinitionOutput, LanguageServerFailureReason>.Success( new DefinitionOutput( links ) );
        }
        catch( Exception e )
        {
            return Result<DefinitionOutput, LanguageServerFailureReason>.Failure( LanguageServerFailureReason.Other, e );
        }
    }
}
