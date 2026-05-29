using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.LanguageServer.UseCase.Abstractions;
using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Compilation;
using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Renaming;
using KSPCompiler.Features.LanguageServer.UseCase.Ast;
using KSPCompiler.Shared;
using KSPCompiler.Shared.Text;

using RenamingResult
    = System.Collections.Generic.Dictionary<KSPCompiler.Features.LanguageServer.UseCase.Abstractions.ScriptLocation, System.Collections.Generic.List<KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Renaming.RenamingItem>>;

namespace KSPCompiler.Features.LanguageServer.UseCase.Renaming;

public sealed class RenamingInteractor : IRenamingUseCase
{
    public async Task<Result<RenamingOutput, LanguageServerFailureReason>> ExecuteAsync( RenamingInput input, CancellationToken cancellationToken = default )
    {
        try
        {
            var compilationCacheManager = input.Cache;
            var scriptLocation = input.Location;
            var position = input.Position;
            var newName = input.NewName;

            var cache = compilationCacheManager.GetCache( scriptLocation );
            var orgName = DocumentUtility.ExtractWord( cache.AllLinesText, position );
            var changes = new RenamingResult();

            var variableFinder = new VariableSymbolAppearanceFinder( orgName );
            var variableAppearances = variableFinder.Find( cache.Ast );

            var functionFinder = new UserFunctionSymbolAppearanceFinder( orgName );
            var functionAppearances = functionFinder.Find( cache.Ast );

            BuildChanges( scriptLocation, newName, variableAppearances, changes );
            BuildChanges( scriptLocation, newName, functionAppearances, changes );

            await Task.CompletedTask;

            return Result<RenamingOutput, LanguageServerFailureReason>.Success( new RenamingOutput( changes ) );
        }
        catch( Exception e )
        {
            return Result<RenamingOutput, LanguageServerFailureReason>.Failure( LanguageServerFailureReason.Other, e );
        }
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
        ICompilationCacheManager compilerCacheService,
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
