using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Applications.LSPServer.Core.Ast;
using KSPCompiler.Applications.LSPServer.Core.Compilation;
using KSPCompiler.Commons.Text;

using RenamingResult = System.Collections.Generic.Dictionary<KSPCompiler.Applications.LSPServer.Core.ScriptLocation, System.Collections.Generic.List<KSPCompiler.Applications.LSPServer.Core.Renaming.RenamingItem>>;

namespace KSPCompiler.Applications.LSPServer.Core.Renaming;

public sealed class RenameHandlingService
{
    public async Task<RenamingResult> HandleAsync(
        CompilationCacheManager compilerCacheService,
        ScriptLocation scriptLocation,
        Position position,
        string newName,
        CancellationToken _ )
    {
        var cache = compilerCacheService.GetCache( scriptLocation );
        var orgName = DocumentUtility.ExtractWord( cache.AllLinesText, position );
        var changes = new RenamingResult();

        var variableFinder = new VariableSymbolAppearanceFinder( orgName );
        var variableAppearances = variableFinder.Find( cache.Ast );

        var functionFinder = new UserFunctionSymbolAppearanceFinder( orgName );
        var functionAppearances = functionFinder.Find( cache.Ast );

        BuildChanges( scriptLocation, newName, variableAppearances, changes );
        BuildChanges( scriptLocation, newName, functionAppearances, changes );

        await Task.CompletedTask;

        return changes;
    }

    private static void BuildChanges( ScriptLocation uri, string newName, IEnumerable<Position> appearances, RenamingResult changes )
    {
        foreach( var x in appearances )
        {
            var textEdit = new RenamingItem
            {
                NewText = newName,
                Range   = x
            };

            if( changes.ContainsKey( uri ) )
            {
                changes[ uri ].Add( textEdit );
            }
            else
            {
                changes.Add( uri, [textEdit] );
            }
        }
    }

    public async Task<(bool result, Position position)> HandlePrepareAsync(
        CompilationCacheManager compilerCacheService,
        ScriptLocation scriptLocation,
        Position position,
        CancellationToken _ )
    {
        var cache = compilerCacheService.GetCache( scriptLocation );
        var orgName = DocumentUtility.ExtractWord( cache.AllLinesText, position );
        var range = DocumentUtility.ExtractWordRange( cache.AllLinesText, position );

        // 対象はユーザー定義変数 or ユーザー定義関数

        var variableFinder = new VariableSymbolAppearanceFinder( orgName );
        var variableAppearances = variableFinder.Find( cache.Ast );

        if( variableAppearances.Any() )
        {
            return ( true, range );
        }

        var functionFinder = new UserFunctionSymbolAppearanceFinder( orgName );
        var functionAppearances = functionFinder.Find( cache.Ast );

        if( functionAppearances.Any() )
        {
            return ( true, range );
        }

        await Task.CompletedTask;

        return ( false, default );
    }
}
