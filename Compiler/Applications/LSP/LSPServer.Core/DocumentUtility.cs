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

    public static string ExtractWord( IReadOnlyList<string> lines, Position position )
    {
        var line = lines[ position.Line ];
        var length = line.Length;
        var start = position.Character;
        var end = position.Character;


        while ( start > 0 && IsIdentifierChar( line, start - 1 ) )
        {
            start--;
        }

        while ( end < length && IsIdentifierChar( line, end ) )
        {
            end++;
        }

        return line.Substring( start, end - start ).Trim();
    }
}
