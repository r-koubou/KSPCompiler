using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.LSPServer.Core.Compilations;

using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

using Range = OmniSharp.Extensions.LanguageServer.Protocol.Models.Range;

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
        var lineIndex = -1;

        var regex = new Regex( $@"\b{orgName}\b" );

        if( DataTypeUtility.IsDataTypeCharacter( orgName[ 0 ] ) )
        {
            regex = new Regex( $@"\{orgName}\b" );
        }

        foreach( var line in cache.AllLinesText )
        {
            lineIndex++;

            var newLine = line;
            var orgLineLength = line.Length;
            var matches = regex.Matches( line );

            if( matches.Count == 0 )
            {
                continue;
            }

            foreach( Match m in matches )
            {
                newLine = regex.Replace( newLine, newName );
            }

            var textEdit = new TextEdit
            {
                NewText = newLine,
                Range = new Range(
                    new Position( lineIndex, 0 ),
                    new Position( lineIndex, orgLineLength )
                )
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

        await Task.CompletedTask;

        return new WorkspaceEdit
        {
            Changes = changes
        };
    }

    public async Task<RangeOrPlaceholderRange?> HandleAsync( PrepareRenameParams request, CancellationToken cancellationToken )
    {
        var cache = CompilerCacheService.GetCache( request.TextDocument.Uri );
        var range = DocumentUtility.ExtractWordRange( cache.AllLinesText, request.Position );

        await Task.CompletedTask;

        return new RangeOrPlaceholderRange( range );
    }
}
