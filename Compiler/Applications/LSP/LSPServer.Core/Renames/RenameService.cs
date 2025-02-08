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

namespace KSPCompiler.Applications.LSPServer.Core.Renames;

public sealed class RenameService( CompilerCacheService compilerCacheService )
{
    private CompilerCacheService CompilerCacheService { get; } = compilerCacheService;

    public async Task<WorkspaceEdit?> HandleAsync( RenameParams request, CancellationToken cancellationToken )
    {
        var uri = request.TextDocument.Uri;
        var cache = CompilerCacheService.GetCache( request.TextDocument.Uri );
        var orgName = DocumentUtility.ExtractWord( cache.AllLinesText, request.Position );
        var newName = request.NewName;
        var changes = new Dictionary<DocumentUri, IEnumerable<TextEdit>>();

        var variableFinder = new VariableSymbolAppearanceFinder( orgName );
        var variableAppearances = variableFinder.Find( cache.Ast );

        var functionFinder = new UserFunctionSymbolAppearanceFinder( orgName );
        var functionAppearances = functionFinder.Find( cache.Ast );

        BuildChanges( uri, orgName, newName, variableAppearances, changes );
        BuildChanges( uri, orgName, newName, functionAppearances, changes );

        await Task.CompletedTask;

        return new WorkspaceEdit
        {
            Changes = changes
        };
    }

    private static void BuildChanges( DocumentUri uri, string orgName, string newName, IEnumerable<DomainPosition> appearances, Dictionary<DocumentUri, IEnumerable<TextEdit>> changes )
    {
        foreach( var x in appearances )
        {
            var textEdit = new TextEdit
            {
                NewText = newName,
                Range   = x.AsRange()
            };

            if( changes.TryGetValue( uri, out var edits ) )
            {
                changes[ uri ] = edits.Append( textEdit );
            }
            else
            {
                changes.Add( uri, [textEdit] );
            }
        }
    }

    public async Task<RangeOrPlaceholderRange?> HandleAsync( PrepareRenameParams request, CancellationToken cancellationToken )
    {
        var cache = CompilerCacheService.GetCache( request.TextDocument.Uri );
        var orgName = DocumentUtility.ExtractWord( cache.AllLinesText, request.Position );
        var range = DocumentUtility.ExtractWordRange( cache.AllLinesText, request.Position );

        // 対象はユーザー定義変数 or ユーザー定義関数

        var variableFinder = new VariableSymbolAppearanceFinder( orgName );
        var variableAppearances = variableFinder.Find( cache.Ast );

        if( variableAppearances.Any() )
        {
            return new RangeOrPlaceholderRange( range );
        }

        var functionFinder = new UserFunctionSymbolAppearanceFinder( orgName );
        var functionAppearances = functionFinder.Find( cache.Ast );

        if( functionAppearances.Any() )
        {
            return new RangeOrPlaceholderRange( range );
        }

        await Task.CompletedTask;

        return null;
    }
}
