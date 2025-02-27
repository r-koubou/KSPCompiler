using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Applications.LSPServer.Core.Ast;
using KSPCompiler.Applications.LSPServer.Core.Compilation;
using KSPCompiler.Commons.Text;

namespace KSPCompiler.Applications.LSPServer.Core.FindReferences;

public sealed class ReferenceHandlingService
{
    public async Task<List<ReferenceItem>> HandleAsync(
        CompilationCacheManager compilerCacheService,
        ScriptLocation scriptLocation,
        Position position,
        CancellationToken _ )
    {
        var cache = compilerCacheService.GetCache( scriptLocation );
        var word = DocumentUtility.ExtractWord( cache.AllLinesText, position );
        var references = new List<ReferenceItem>();

        var variableFinder = new VariableSymbolAppearanceFinder( word, mode:AppearanceFinderMode.All );
        var variableAppearances = variableFinder.Find( cache.Ast );

        var functionFinder = new UserFunctionSymbolAppearanceFinder( word, AppearanceFinderMode.All );
        var functionAppearances = functionFinder.Find( cache.Ast );

        BuildReferences( scriptLocation, variableAppearances, references );
        BuildReferences( scriptLocation, functionAppearances, references );

        await Task.CompletedTask;

        return references;
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
