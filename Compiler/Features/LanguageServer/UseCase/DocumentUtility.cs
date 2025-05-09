using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData;
using KSPCompiler.Shared.Text;

namespace KSPCompiler.Features.LanguageServer.UseCase;

internal static class DocumentUtility
{
    // ReSharper disable once MemberCanBePrivate.Global
    public static bool IsIdentifierChar( char c )
    {
        return c == '_'
               || char.IsLetterOrDigit( c )
               || DataTypeUtility.IsDataTypeCharacter( c );
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public static bool IsIdentifierChar( string text, int index )
    {
        return IsIdentifierChar( text[ index ] );
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public static bool IsSkipChar( char c )
    {
        return char.IsWhiteSpace( c )
               || char.IsControl( c );
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public static bool IsSkipChar( string text, int index )
    {
        return IsSkipChar( text[ index ] );
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public static(int start, int end) GetWordRange( string line, int column )
    {
        var length = line.Length;
        var start = column;
        var end = column;

        while( start > 0 )
        {
            if( IsIdentifierChar( line, start - 1 ) )
            {
                start--;
            }
            else
            {
                break;
            }
        }

        while( end < length )
        {
            if( IsIdentifierChar( line, end ) )
            {
                end++;
            }
            else
            {
                break;
            }
        }

        // while( start > 0 && IsIdentifierChar( line, start - 1 ) )
        // {
        //     start--;
        // }
        //
        // while( end < length && IsIdentifierChar( line, end ) )
        // {
        //     end++;
        // }

        return ( start, end );
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public static string ExtractWord( IReadOnlyList<string> lines, Position position )
    {
        if( lines.Count == 0 )
        {
            return string.Empty;
        }

        var line = lines[ position.BeginLine.Value - 1 ]; // 0-based
        var (start, end) = GetWordRange( line, position.BeginColumn.Value );

        return line.Substring( start, end - start ).Trim();
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public static Position ExtractWordRange( IReadOnlyList<string> lines, Position position )
    {
        if( lines.Count == 0 )
        {
            return Position.Zero;
        }

        var line = lines[ position.BeginLine.Value ];
        var (start, end) = GetWordRange( line, position.BeginColumn.Value );

        return new Position()
        {
            BeginLine   = position.BeginLine,
            BeginColumn = start,
            EndLine     = position.BeginLine,
            EndColumn   = end
        };
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public static bool IsInCommentToLeft( IReadOnlyList<string> lines, Position position )
    {
        if( lines.Count == 0 )
        {
            return false;
        }

        var begin = position.BeginLine.Value - 1; // 0-based
        var beginColumn = position.BeginColumn.Value;

        for( var line = begin; line >= 0; line-- )
        {
            var text = lines[ line ];
            var textLength = text.Length;
            var startColumn = line == begin ? beginColumn : textLength - 1;

            // If column in EOL, set to last column
            startColumn = Math.Min( textLength - 1, startColumn );

            for( var column = startColumn; column >= 0; column-- )
            {
                //Console.Error.WriteLine($"line: {line}, column: {column}, textLength:{textLength}, text:{text}");

                if( text[ column ] == '{' )
                {
                    return true;
                }
                // コメント内にネストしたコメントを書けない文法のため
                // コメント終了文字が見つかったらコメント外とみなす
                // Due to a grammar that does not allow nested comments within comments,
                // if a comment end character is found, it is considered out of comment.
                if( text[ column ] == '}' )
                {
                    return false;
                }
            }
        }

        return false;
    }

    public static bool IsInCommentToRight( IReadOnlyList<string> lines, Position position )
    {
        if( lines.Count == 0 )
        {
            return false;
        }

        var begin = position.BeginLine.Value - 1; // 0-based
        var beginColumn = position.BeginColumn.Value;
        var lineCount = lines.Count;

        for( var line = begin; line < lineCount; line++ )
        {
            var text = lines[ line ];
            var startColumn = line == begin  ? beginColumn : 0;
            var textLength = text.Length;

            for( var column = startColumn; column < textLength; column++ )
            {
                if( text[ column ] == '}' )
                {
                    return true;
                }
                // コメント内にネストしたコメントを書けない文法のため
                // コメント開始文字が見つかったらコメント外とみなす
                // Due to a grammar that does not allow nested comments within comments,
                // if a comment start character is found, it is considered out of comment.
                if( text[ column ] == '{' )
                {
                    return false;
                }
            }
        }

        return false;
    }

    public static string GetCommentOrDescriptionText( SymbolBase symbol )
    {
        var commentLines = symbol.CommentLines;

        if( commentLines.Count > 0 )
        {
            var builder = new StringBuilder( 128 );
            var minIndent = commentLines
                           .Where( line => line.Trim().Length > 0 )
                           .Select( line => line.Length - line.TrimStart().Length )
                           .DefaultIfEmpty( 0 )
                           .Min();

            var normalizedLines = commentLines
               .Select( line => line.Length >= minIndent ? line.Substring( minIndent ) : line );

            foreach( var line in normalizedLines )
            {
                builder.AppendLine( line );
            }

            return builder.ToString();
        }

        return symbol.Description.Value;
    }
}
