using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KSPCompiler.Domain.Symbols;

public interface ISymbolExporter<TSymbol> where TSymbol : SymbolBase
{
    /// <summary>
    /// Export symbol table to repository.
    /// </summary>
    void Export( IEnumerable<TSymbol> symbols )
        => ExportAsync( symbols ).GetAwaiter().GetResult();

    /// <summary>
    /// Export symbol table to repository.
    /// </summary>
    Task ExportAsync( IEnumerable<TSymbol> store, CancellationToken cancellationToken = default );
}
