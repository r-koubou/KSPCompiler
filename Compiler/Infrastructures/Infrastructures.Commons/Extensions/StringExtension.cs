using System.Collections.Generic;

namespace KSPCompiler.Infrastructures.Commons.Extensions;

public static class StringExtension
{
    public static IReadOnlyCollection<string> SplitNewLine( this string me )
    {
        return me.Split( '\n', '\r' );
    }
}
