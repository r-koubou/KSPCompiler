using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Applications.LSPServer.Core.Compilation;
using KSPCompiler.Applications.LSPServer.Core.Extensions;
using KSPCompiler.Commons.Text;

namespace KSPCompiler.Applications.LSPServer.Core.Definition;

public sealed class DefinitionHandlingService
{
    public async Task<List<LocationLink>> HandleAsync(
        CompilationCacheManager compilationCacheManager,
        ScriptLocation scriptLocation,
        Position position,
        CancellationToken _ )
    {
        if( !compilationCacheManager.ContainsCache( scriptLocation ) )
        {
            return [];
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
        return links;
    }
}
