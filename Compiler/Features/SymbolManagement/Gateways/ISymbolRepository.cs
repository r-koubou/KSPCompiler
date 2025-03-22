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

    StoreResult Store( TSymbol symbol )
        => StoreAsync( symbol ).GetAwaiter().GetResult();

    Task<StoreResult> StoreAsync( TSymbol symbol, CancellationToken cancellationToken = default );

    StoreResult Store( IEnumerable<TSymbol> symbols )
        => StoreAsync( symbols ).GetAwaiter().GetResult();

    Task<StoreResult> StoreAsync( IEnumerable<TSymbol> symbols, CancellationToken cancellationToken = default );

    DeleteResult Delete( TSymbol symbol )
        => DeleteAsync( symbol ).GetAwaiter().GetResult();

    Task<DeleteResult> DeleteAsync( TSymbol symbol, CancellationToken cancellationToken = default );

    DeleteResult Delete( IEnumerable<TSymbol> symbols )
        => DeleteAsync( symbols ).GetAwaiter().GetResult();

    Task<DeleteResult> DeleteAsync( IEnumerable<TSymbol> symbols, CancellationToken cancellationToken = default );

    IReadOnlyCollection<TSymbol> FindByName( string name )
        => FindByNameAsync( name ).GetAwaiter().GetResult();

    Task<IReadOnlyCollection<TSymbol>>  FindByNameAsync( string name, CancellationToken cancellationToken = default );

    IReadOnlyCollection<TSymbol> Find( Predicate<TSymbol> predicate )
        => FindAsync( predicate ).GetAwaiter().GetResult();

    Task<IReadOnlyCollection<TSymbol>> FindAsync( Predicate<TSymbol> predicate, CancellationToken cancellationToken = default );

    void Flush()
        => FlushAsync().GetAwaiter().GetResult();

    async Task FlushAsync()
    {
        await Task.CompletedTask;
    }
}
