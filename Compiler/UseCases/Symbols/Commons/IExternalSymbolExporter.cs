using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.UseCases.Symbols.Commons;

public interface IExternalSymbolExporter<TSymbol> where TSymbol : SymbolBase
{
    /// <summary>
    /// Export symbol table to repository.
    /// </summary>
    void Export( ISymbolTable<TSymbol> store )
        => ExportAsync( store ).GetAwaiter().GetResult();

    /// <summary>
    /// Export symbol table to repository.
    /// </summary>
    Task ExportAsync( ISymbolTable<TSymbol> store, CancellationToken cancellationToken = default );
}
