using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KSPCompiler.Domain.Symbols;

public interface ISymbolImporter<TSymbol> where TSymbol : SymbolBase
{
    /// <summary>
    /// Null Object. Always return empty symbol table.
    /// </summary>
    public static ISymbolImporter<TSymbol> Null { get; } = new NullImporter();

    /// <summary>
    /// Import symbol table from repository.
    /// </summary>
    /// <returns>
    /// A symbol table when success, otherwise throw exception.
    /// </returns>
    IReadOnlyCollection<TSymbol> Import()
        => ImportAsync().GetAwaiter().GetResult();

    /// <summary>
    /// Import symbol table from repository asynchronously.
    /// </summary>
    Task<IReadOnlyCollection<TSymbol>> ImportAsync( CancellationToken cancellationToken = default );

    private class NullImporter : ISymbolImporter<TSymbol>
    {
        public async Task<IReadOnlyCollection<TSymbol>> ImportAsync( CancellationToken cancellationToken = default )
        {
            await Task.CompletedTask;

            return new List<TSymbol>();
        }
    }
}
