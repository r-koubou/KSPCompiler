using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.UseCases.Symbols.Commons;

public interface IExternalSymbolImporter<TSymbol> where TSymbol : SymbolBase
{
    /// <summary>
    /// Import symbol table from repository.
    /// </summary>
    /// <returns>
    /// A symbol table when success, otherwise throw exception.
    /// </returns>
    public ISymbolTable<TSymbol> Import()
        => ImportAsync().GetAwaiter().GetResult();

    /// <summary>
    /// Import symbol table from repository asynchronously.
    /// </summary>
    Task<ISymbolTable<TSymbol>> ImportAsync( CancellationToken cancellationToken = default );
}
