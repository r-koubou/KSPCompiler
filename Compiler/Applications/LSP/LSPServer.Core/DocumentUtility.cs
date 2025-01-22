using System.Collections.Generic;

using KSPCompiler.Domain.Symbols.MetaData;

using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace KSPCompiler.LSPServer.Core;

public static class DocumentUtility
{
    private static bool IsIdentifierChar( string text, int index )
    {
        var c = text[ index ];

        return c == '_'
               || char.IsLetterOrDigit( c )
               || DataTypeUtility.IsDataTypeCharacter( c );
    }

    public static(int start, int end) GetWordRange( string line, int column )
    {
        var length = line.Length;
        var start = column;
        var end = column;

        while( start > 0 && IsIdentifierChar( line, start - 1 ) )
        {
            start--;
        }

        while( end < length && IsIdentifierChar( line, end ) )
        {
            end++;
        }

        return ( start, end );
    }

    public static string ExtractWord( IReadOnlyList<string> lines, Position position )
    {
        var line = lines[ position.Line ];
        var (start, end) = GetWordRange( line, position.Character );

        return line.Substring( start, end - start ).Trim();
    }

    public static Range ExtractWordRange( IReadOnlyList<string> lines, Position position )
    {
        var line = lines[ position.Line ];
        var (start, end) = GetWordRange( line, position.Character );

        return new Range(
            new Position( position.Line, start ),
            new Position( position.Line, end )
        );
    }

    public static bool IsInCommentToLeft( IReadOnlyList<string> lines, Position position )
    {
        var begin = position.Line;
        var beginColumn = position.Character - 1;

        for( var line = begin; line >= 0; line-- )
        {
            var text = lines[ line ];
            var startColumn = line == begin ? beginColumn : text.Length - 1;

            for( var column = startColumn; column >= 0; column-- )
            {
                if( text[ column ] == '{' )
                {
                    return true;
                }
            }
        }

        return false;
    }

    public static bool IsInCommentToRight( IReadOnlyList<string> lines, Position position )
    {
        var begin = position.Line;
        var beginColumn = position.Character;
        var first = true;

        for( var line = begin; line < lines.Count; line++ )
        {
            var text = lines[ line ];
            var startColumn = first ? beginColumn : 0;

            for( var column = startColumn; column < text.Length; column++ )
            {
                if( text[ column ] == '}' )
                {
                    return true;
                }
            }
        }

        return false;
    }
}
