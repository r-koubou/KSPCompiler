using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Commons.Text;
using KSPCompiler.UseCases.LanguageServer.SignatureHelp;
using KSPCompiler.UseCases.LanguageServer.SignatureHelp.Extensions;

namespace KSPCompiler.Interactors.LanguageServer.SignatureHelp;

public sealed class SignatureHelpInteractor : ISignatureHelpUseCase
{
    public async Task<SignatureHelpOutputPort> ExecuteAsync( SignatureHelpInputPort parameter, CancellationToken cancellationToken = default )
    {
        try
        {
            var compilationCacheManager = parameter.Input.Cache;
            var scriptLocation = parameter.Input.Location;
            var position = parameter.Input.Position;

            var cache = compilationCacheManager.GetCache( scriptLocation );
            var symbols = cache.SymbolTable;

            if( DocumentUtility.IsInCommentToLeft( cache.AllLinesText, position ) )
            {
                return new SignatureHelpOutputPort( null, true );
            }

            // var word = ExtractCallCommandName( cache.AllLinesText, position );
            // var activeParameter = GetCallCommandActiveArgument( cache.AllLinesText, position );
            var iterator = new BackwardIterator(
                cache.AllLinesText,
                position.BeginLine.Value - 1,     // 0-based
                position.BeginColumn.Value - 1 // 0-based and caret position - 1
            );

            var activeParameter = NewGetCallCommandActiveArgument( iterator );

            if( activeParameter < 0 )
            {
                return new SignatureHelpOutputPort( null, true );
            }

            var word = GetIdentifier( iterator );

            if( string.IsNullOrEmpty( word ) )
            {
                return new SignatureHelpOutputPort( null, true );
            }

            if( !symbols.Commands.TryBuildSignatureHelp( word, activeParameter, out var signatureHelp ) )
            {
                return new SignatureHelpOutputPort( null, true );
            }

            await Task.CompletedTask;

            return new SignatureHelpOutputPort( signatureHelp, true );
        }
        catch( Exception e )
        {
            return new SignatureHelpOutputPort( null, false, e );
        }
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

    //--------------------------------------------------------------------------------------------------------
    // Implemented based on Part of PHP Signature Help Provider implementation. (signatureHelpProvider.ts)
    // https://github.com/microsoft/vscode
    //--------------------------------------------------------------------------------------------------------

    private static int NewGetCallCommandActiveArgument( BackwardIterator iterator )
    {
        var parentNestDepth = 0;
        var bracketNestDepth = 0;
        var activeArgument = 0;

        while( iterator.HasNext )
        {
            var c = iterator.GetNext();

            switch( c )
            {
                case '(':
                    parentNestDepth--;

                    if( parentNestDepth < 0 )
                    {
                        return activeArgument;
                    }

                    break;
                case ')':
                    parentNestDepth++;

                    break;

                case '[':
                    bracketNestDepth--;

                    break;

                case ']':
                    bracketNestDepth++;

                    break;
/*
                case '\'':
                case '\"':
                    while( iterator.HasNext )
                    {
                        var c2 = iterator.GetNext();

                        if( c2 == c )
                        {
                            break;
                        }
                    }

                    break;
*/
                case ',':
                    if( parentNestDepth == 0 && bracketNestDepth == 0 )
                    {
                        activeArgument++;
                    }
                    break;
            }
        }

        return -1;
    }

    private static string GetIdentifier( BackwardIterator iterator )
    {
        var identStarted = false;
        var ident = string.Empty;

        while( iterator.HasNext )
        {
            var ch = iterator.GetNext();

            if( !identStarted && DocumentUtility.IsSkipChar( ch ) )
            {
                continue;
            }

            if( DocumentUtility.IsIdentifierChar( ch ) )
            {
                identStarted = true;
                ident        = ch + ident;
            }
            else if( identStarted )
            {
                return ident;
            }
        }

        return ident;
    }
}

internal class BackwardIterator(
    IReadOnlyList<string> lines,
    int beginLine,
    int beginColumn
)
{
    private readonly IReadOnlyList<string> lines = lines;
    private int lineIndex = beginLine;
    private int columnIndex = beginColumn;

    public bool HasNext
        => lineIndex >= 0;

    public char GetNext()
    {
        if( columnIndex < 0 )
        {
            if( lineIndex > 0 )
            {

                lineIndex--;
                columnIndex = lines[ lineIndex ].Length - 1;

                return '\n';
            }

            lineIndex = -1;

            return '\0';
        }

        var c = lines[ lineIndex ][ columnIndex ];
        columnIndex--;

        return c;
    }
}
