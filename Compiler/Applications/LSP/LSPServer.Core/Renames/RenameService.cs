using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.LSPServer.Core.Ast;
using KSPCompiler.LSPServer.Core.Compilations;
using KSPCompiler.LSPServer.Core.Extensions;

using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

using DomainPosition = KSPCompiler.Commons.Text.Position;

namespace KSPCompiler.LSPServer.Core.Renames;

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

        var variableFinder = new VariableSymbolAppearanceFinder( orgName, cache.SymbolTable.UserVariables );
        var variableAppearances = variableFinder.Find( cache.Ast );

        BuildChanges( uri, orgName, newName, variableAppearances, changes );

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
        var range = DocumentUtility.ExtractWordRange( cache.AllLinesText, request.Position );

        await Task.CompletedTask;

        return new RangeOrPlaceholderRange( range );
    }
}
