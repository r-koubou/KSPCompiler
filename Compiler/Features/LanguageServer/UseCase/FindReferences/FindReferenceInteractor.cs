using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.LanguageServer.UseCase.Abstractions;
using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.FindReferences;
using KSPCompiler.Features.LanguageServer.UseCase.Ast;
using KSPCompiler.Shared;
using KSPCompiler.Shared.Text;

namespace KSPCompiler.Features.LanguageServer.UseCase.FindReferences;

public sealed class FindReferenceInteractor : IFindReferenceUseCase
{
    public async Task<Result<FindReferenceOutput, LanguageServerFailureReason>> ExecuteAsync(
        FindReferenceInput input,
        CancellationToken cancellationToken = default )
    {
        try
        {
            var compilationCacheManager = input.Cache;
            var scriptLocation = input.Location;
            var position = input.Position;

            var cache = compilationCacheManager.GetCache( scriptLocation );
            var word = DocumentUtility.ExtractWord( cache.AllLinesText, position );
            var references = new List<ReferenceItem>();

            var variableFinder = new VariableSymbolAppearanceFinder( word, mode:AppearanceFinderMode.All );
            var variableAppearances = variableFinder.Find( cache.Ast );

            var functionFinder = new UserFunctionSymbolAppearanceFinder( word, AppearanceFinderMode.All );
            var functionAppearances = functionFinder.Find( cache.Ast );

            BuildReferences( scriptLocation, variableAppearances, references );
            BuildReferences( scriptLocation, functionAppearances, references );

            await Task.CompletedTask;

            return Result<FindReferenceOutput, LanguageServerFailureReason>.Success( new FindReferenceOutput( references ) );
        }
        catch( Exception e )
        {
            return Result<FindReferenceOutput, LanguageServerFailureReason>.Failure( LanguageServerFailureReason.Other, e );
        }
    }

    private static void BuildReferences( ScriptLocation location, IEnumerable<Position> appearances, List<ReferenceItem> locations )
    {
        foreach( var x in appearances )
        {
            locations.Add( new ReferenceItem
                {
                    Location = location,
                    Range    = x
                }
            );
        }
    }

}
