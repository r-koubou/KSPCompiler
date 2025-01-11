using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.LSPServer.Core.Compilations;
using KSPCompiler.LSPServer.Core.Extensions;

using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace KSPCompiler.LSPServer.Core.Definitions;

public sealed class DefinitionService( CompilerCacheService compilerCacheService )
{
    private CompilerCacheService CompilerCacheService { get; } = compilerCacheService;

    public async Task<LocationOrLocationLinks?> HandleAsync( DefinitionParams request, CancellationToken cancellationToken )
    {
        var cache = CompilerCacheService.GetCache( request.TextDocument.Uri );
        var word = DocumentUtility.ExtractWord( cache.AllLinesText, request.Position );
        var links = new List<LocationOrLocationLink>();

        // ユーザー定義変数
        if( cache.SymbolTable.UserVariables.TrySearchDefinitionLocation( request.TextDocument.Uri, word, out var result ) )
        {
            links.Add( new LocationOrLocationLink( result ) );
        }

        // ユーザー定義関数
        if( cache.SymbolTable.UserFunctions.TrySearchDefinitionLocation( request.TextDocument.Uri, word, out result ) )
        {
            links.Add( new LocationOrLocationLink( result ) );
        }

        if( links.Any() )
        {
            return new LocationOrLocationLinks( links );
        }

        await Task.CompletedTask;
        return null;
    }
}
