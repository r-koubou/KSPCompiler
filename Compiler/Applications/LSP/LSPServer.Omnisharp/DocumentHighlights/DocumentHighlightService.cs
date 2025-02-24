using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Applications.LSPServer.Omnisharp.Compilations;
using KSPCompiler.Applications.LSPServer.Omnisharp.Extensions;

using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace KSPCompiler.Applications.LSPServer.Omnisharp.DocumentHighlights;

public sealed class DocumentHighlightService( CompilerCacheService compilerCacheService )
{
    private CompilerCacheService CompilerCacheService { get; } = compilerCacheService;

    public async Task<DocumentHighlightContainer?> HandleAsync( DocumentHighlightParams request, CancellationToken cancellationToken )
    {
        var cache = CompilerCacheService.GetCache( request.TextDocument.Uri );
        var word = DocumentUtility.ExtractWord( cache.AllLinesText, request.Position );
        var highlights = new List<DocumentHighlight>();

        // ユーザー定義変数
        if( cache.SymbolTable.UserVariables.TrySearchByName( word, out var variable ) )
        {
            var finder = new DocumentHighlightFinder( cache.Ast, cache.SymbolTable, variable );

            highlights.Add(
                new DocumentHighlight
                {
                    Range = variable.DefinedPosition.AsRange(),
                    Kind  = DocumentHighlightKind.Text
                }
            );

            highlights.AddRange( finder.Find() );
        }
        else if( cache.SymbolTable.UserFunctions.TrySearchByName( word, out var function ) )
        {
            var finder = new DocumentHighlightFinder( cache.Ast, cache.SymbolTable, function );

            highlights.Add(
                new DocumentHighlight
                {
                    Range = function.DefinedPosition.AsRange(),
                    Kind  = DocumentHighlightKind.Text
                }
            );

            highlights.AddRange( finder.Find() );
        }

        await Task.CompletedTask;

        return new DocumentHighlightContainer( highlights );
    }
}
