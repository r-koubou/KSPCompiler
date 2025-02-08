using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Applications.LSPServer.Core.Ast;
using KSPCompiler.Applications.LSPServer.Core.Compilations;
using KSPCompiler.Applications.LSPServer.Core.Extensions;

using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

using DomainPosition = KSPCompiler.Commons.Text.Position;

namespace KSPCompiler.Applications.LSPServer.Core.References;

public class ReferencesService( CompilerCacheService compilerCacheService )
{
    private CompilerCacheService CompilerCacheService { get; } = compilerCacheService;

    public async Task<LocationContainer?> HandleAsync( ReferenceParams request, CancellationToken cancellationToken )
    {
        var uri = request.TextDocument.Uri;
        var cache = CompilerCacheService.GetCache( request.TextDocument.Uri );
        var word = DocumentUtility.ExtractWord( cache.AllLinesText, request.Position );
        var locations = new List<Location>();

        var variableFinder = new VariableSymbolAppearanceFinder( word, mode:AppearanceFinderMode.Reference );
        var variableAppearances = variableFinder.Find( cache.Ast );

        var functionFinder = new UserFunctionSymbolAppearanceFinder( word, AppearanceFinderMode.Reference );
        var functionAppearances = functionFinder.Find( cache.Ast );

        BuildReferences( uri, variableAppearances, locations );
        BuildReferences( uri, functionAppearances, locations );

        await Task.CompletedTask;

        return !locations.Any() ? null : new LocationContainer( locations );
    }

    private static void BuildReferences( DocumentUri uri, IEnumerable<DomainPosition> appearances, List<Location> locations )
    {
        foreach( var x in appearances )
        {
            locations.Add( new Location
                {
                    Uri   = uri,
                    Range = x.AsRange()
                }
            );
        }
    }
}
