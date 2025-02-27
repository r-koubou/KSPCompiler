using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Commons.Text;
using KSPCompiler.Interactors.LanguageServer.Ast;
using KSPCompiler.UseCases.LanguageServer;
using KSPCompiler.UseCases.LanguageServer.FindReferences;

namespace KSPCompiler.Interactors.LanguageServer.FindReferences;

public sealed class FindReferenceInteractor : IFindReferenceUseCase
{
    public async Task<FindReferenceOutputPort> ExecuteAsync(
        FindReferenceInputPort parameter,
        CancellationToken cancellationToken = default )
    {
        try
        {
            var compilationCacheManager = parameter.Data.Cache;
            var scriptLocation = parameter.Data.Location;
            var position = parameter.Data.Position;

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
