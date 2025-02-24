using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Applications.LSPServer.CoreNew.Compilation;
using KSPCompiler.Applications.LSPServer.CoreNew.SignatureHelp.Extensions;
using KSPCompiler.Commons.Text;

namespace KSPCompiler.Applications.LSPServer.CoreNew.SignatureHelp;

public sealed class SignatureHelpHandlingService
{
    public async Task<SignatureHelpItem?> HandleAsync(
        CompilationCacheManager compilerCacheService,
        ScriptLocation scriptLocation,
        Position position,
        CancellationToken _ )
    {
        var cache = compilerCacheService.GetCache( scriptLocation );
        var symbols = cache.SymbolTable;

        if( DocumentUtility.IsInCommentToLeft( cache.AllLinesText, position ) )
        {
            return null;
        }

        var word = ExtractCallCommandName( cache.AllLinesText, position );
        var activeParameter = GetCallCommandActiveArgument( cache.AllLinesText, position );

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
        var line = lines[ position.BeginLine.Value - 1 ];
        var start = position.BeginColumn.Value; // caret position - 1

        // カーソル位置がEOFの場合、最後の文字に設定
        start = Math.Min( line.Length - 1, start );

        // 行頭からコマンド呼び出しの開始位置を探す
        while( start >= 0 && line[ start ] != '(' )
        {
            start--;
        }

        if( start < 0 )
        {
            return string.Empty;
        }

        return DocumentUtility.ExtractWord(
            lines,
            new Position
            {
                BeginLine   = position.BeginLine,
                BeginColumn = start
            }
        );
    }

    private static int GetCallCommandActiveArgument( IReadOnlyList<string> lines, Position position )
    {
        var line = lines[ position.BeginLine.Value - 1 ];
        var length = line.Length;
        var caret = position.BeginColumn.Value;
        var start = position.BeginColumn.Value - 1; // caret position - 1

        // カーソル位置がEOFの場合、最後の文字に設定
        start = Math.Min( line.Length - 1, start );

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

        // カーソル位置までのカンマの数を数える
        for( var i = start + 1; i < caret && i < length; i++ )
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
