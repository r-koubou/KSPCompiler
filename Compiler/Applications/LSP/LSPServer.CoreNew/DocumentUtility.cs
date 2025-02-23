using System.Collections.Generic;

using KSPCompiler.Commons.Text;
using KSPCompiler.Domain.Symbols.MetaData;

namespace KSPCompiler.Applications.LSPServer.CoreNew;

internal static class DocumentUtility
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
        var line = lines[ position.BeginLine.Value - 1 ]; // 0-based
        var (start, end) = GetWordRange( line, position.BeginColumn.Value );

        return line.Substring( start, end - start ).Trim();
    }

    public static Position ExtractWordRange( IReadOnlyList<string> lines, Position position )
    {
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

    public static bool IsInCommentToLeft( IReadOnlyList<string> lines, Position position )
    {
        var begin = position.BeginLine.Value;
        var beginColumn = position.BeginColumn.Value; // 0-based

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
        var begin = position.BeginLine.Value;
        var beginColumn = position.BeginColumn.Value;
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
