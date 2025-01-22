using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.LSPServer.Core.Compilations;
using KSPCompiler.LSPServer.Core.SignatureHelps.Extensions;

using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace KSPCompiler.LSPServer.Core.SignatureHelps;

public sealed class SignatureHelpService( CompilerCacheService compilerCacheService )
{
    private CompilerCacheService CompilerCacheService { get; } = compilerCacheService;

    public async Task<SignatureHelp?> HandleAsync( SignatureHelpParams request, CancellationToken cancellationToken )
    {
        var cache = CompilerCacheService.GetCache( request.TextDocument.Uri );
        var symbols = cache.SymbolTable;
        var word = ExtractCallCommandName( cache.AllLinesText, request.Position );
        var activeParameter = GetCallCommandActiveArgument( cache.AllLinesText, request.Position );

        if( string.IsNullOrEmpty( word ) )
        {
            return null;
        }

        if( !symbols.Commands.TryBuildSignatureHelp( word, activeParameter, out var signatureHelp ) )
        {
            return null;
        }

        await Task.CompletedTask;

        return signatureHelp;
    }

    private static string ExtractCallCommandName( IReadOnlyList<string> lines, Position position )
    {
        var line = lines[ position.Line ];
        var start = position.Character - 1; // zero-based index

        // 行頭からコマンド呼び出しの開始位置を探す
        while( start >= 0 && line[ start ] != '(' )
        {
            start--;
        }

        if( start < 0 )
        {
            return string.Empty;
        }

        return DocumentUtility.ExtractWord( lines, new Position( position.Line, start ) );
    }

    private static int GetCallCommandActiveArgument( IReadOnlyList<string> lines, Position position )
    {
        var line = lines[ position.Line ];
        var length = line.Length;
        var start = position.Character - 1; // zero-based index

        // 行頭からコマンド呼び出しの開始位置を探す
        while( start >= 0 && line[ start ] != '(' )
        {
            start--;
        }

        if( start < 0 )
        {
            return 0;
        }

        var activeArgument = 0;

        for( var i = start + 1; i < length; i++ )
        {
            if( line[ i ] == ',' )
            {
                activeArgument++;
            }
            else if( line[ i ] == ')' )
            {
                break;
            }
        }

        return activeArgument;
    }
}
