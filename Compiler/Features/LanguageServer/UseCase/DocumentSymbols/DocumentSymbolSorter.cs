using System.Collections.Generic;

using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Symbol;

namespace KSPCompiler.Features.LanguageServer.UseCase.DocumentSymbols;

public static class DocumentSymbolSorter
{
    public static void SortByOccurrence( List<DocumentSymbol> symbols )
    {
        symbols.Sort( ( a, b ) =>
            {
                var lineA = a.SelectionRange.BeginLine.Value;
                var lineB = b.SelectionRange.BeginLine.Value;

                var columnA = a.SelectionRange.BeginColumn.Value;
                var columnB = b.SelectionRange.BeginColumn.Value;

                var lineCompare = lineA.CompareTo( lineB );

                return lineCompare != 0
                    ? lineCompare
                    : columnA.CompareTo( columnB );
            }
        );
    }
}
