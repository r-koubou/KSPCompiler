using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.LanguageServer.UseCase.Abstractions;
using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.FindReferences;
using KSPCompiler.Features.LanguageServer.UseCase.Ast;
using KSPCompiler.Shared.Text;

namespace KSPCompiler.Features.LanguageServer.UseCase.FindReferences;

public sealed class FindReferenceInteractor : IFindReferenceUseCase
{
    public async Task<FindReferenceOutputPort> ExecuteAsync(
        FindReferenceInputPort parameter,
        CancellationToken cancellationToken = default )
    {
        try
        {
            var compilationCacheManager = parameter.Input.Cache;
            var scriptLocation = parameter.Input.Location;
            var position = parameter.Input.Position;

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

            return new FindReferenceOutputPort( references, true );
        }
        catch( Exception e )
        {
            return new FindReferenceOutputPort( [ ], false, e );
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
