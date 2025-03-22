using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Shared.Domain.Compilation.Symbols;

namespace KSPCompiler.Features.SymbolManagement.Gateways;

public interface ISymbolRepository<TSymbol> : IDisposable where TSymbol : SymbolBase
{
    int Count { get; }

    List<TSymbol> ToList();
    Task<List<TSymbol>> ToListAsync( CancellationToken cancellationToken = default );

    #region Store
    StoreResult Store( TSymbol symbol );
    Task<StoreResult> StoreAsync( TSymbol symbol, CancellationToken cancellationToken = default );
    StoreResult Store( IEnumerable<TSymbol> symbols );
    Task<StoreResult> StoreAsync( IEnumerable<TSymbol> symbols, CancellationToken cancellationToken = default );
    #endregion ~Store

    #region Delete
    DeleteResult Delete( TSymbol symbol );
    Task<DeleteResult> DeleteAsync( TSymbol symbol, CancellationToken cancellationToken = default );
    DeleteResult Delete( IEnumerable<TSymbol> symbols );
    Task<DeleteResult> DeleteAsync( IEnumerable<TSymbol> symbols, CancellationToken cancellationToken = default );
    #endregion ~Delete

    #region Find
    IReadOnlyCollection<TSymbol> FindByName( string name );
    Task<IReadOnlyCollection<TSymbol>> FindByNameAsync( string name, CancellationToken cancellationToken = default );
    IReadOnlyCollection<TSymbol> Find( Predicate<TSymbol> predicate );
    Task<IReadOnlyCollection<TSymbol>> FindAsync( Predicate<TSymbol> predicate, CancellationToken cancellationToken = default );
    #endregion ~Find

    void Flush()
        => FlushAsync().GetAwaiter().GetResult();

    async Task FlushAsync()
    {
        await Task.CompletedTask;
    }
}
