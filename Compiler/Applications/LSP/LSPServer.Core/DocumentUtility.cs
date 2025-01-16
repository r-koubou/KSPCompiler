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

    public static string ExtractCallCommandName( IReadOnlyList<string> lines, Position position )
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
            return string.Empty;
        }

        return ExtractWord( lines, new Position( position.Line, start ) );
    }
}
