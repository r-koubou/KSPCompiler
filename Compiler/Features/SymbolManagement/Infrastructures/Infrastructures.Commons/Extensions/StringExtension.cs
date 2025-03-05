using System.Collections.Generic;

namespace KSPCompiler.Features.SymbolManagement.Infrastructures.Commons.Extensions;

public static class StringExtension
{
    public static IReadOnlyCollection<string> SplitNewLine( this string me )
    {
        return me.Split( '\n', '\r' );
    }
}
