using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Shared.Domain.Symbols;

namespace KSPCompiler.Features.SymbolManagement.Gateways;

public interface ISymbolRepository<TSymbol> : IDisposable where TSymbol : SymbolBase
{
    public int Count { get; }

    public StoreResult Store( TSymbol symbol )
        => StoreAsync( symbol ).GetAwaiter().GetResult();

    public Task<StoreResult> StoreAsync( TSymbol symbol, CancellationToken cancellationToken = default );

    public StoreResult Store( IEnumerable<TSymbol> symbols )
        => StoreAsync( symbols ).GetAwaiter().GetResult();

    public Task<StoreResult> StoreAsync( IEnumerable<TSymbol> symbols, CancellationToken cancellationToken = default );

    public DeleteResult Delete( TSymbol symbol )
        => DeleteAsync( symbol ).GetAwaiter().GetResult();

    public Task<DeleteResult> DeleteAsync( TSymbol symbol, CancellationToken cancellationToken = default );

    public DeleteResult Delete( IEnumerable<TSymbol> symbols )
        => DeleteAsync( symbols ).GetAwaiter().GetResult();

    public Task<DeleteResult> DeleteAsync( IEnumerable<TSymbol> symbols, CancellationToken cancellationToken = default );

    public IReadOnlyCollection<TSymbol> FindByName( string name )
        => FindByNameAsync( name ).GetAwaiter().GetResult();

    public Task<IReadOnlyCollection<TSymbol>>  FindByNameAsync( string name, CancellationToken cancellationToken = default );

    public IReadOnlyCollection<TSymbol> Find( Predicate<TSymbol> predicate )
        => FindAsync( predicate ).GetAwaiter().GetResult();

    public Task<IReadOnlyCollection<TSymbol>> FindAsync( Predicate<TSymbol> predicate, CancellationToken cancellationToken = default );

    public IReadOnlyCollection<TSymbol> FindAll()
        => FindAllAsync().GetAwaiter().GetResult();

    public Task<IReadOnlyCollection<TSymbol>> FindAllAsync( CancellationToken cancellationToken = default );

    public void Flush()
        => FlushAsync().GetAwaiter().GetResult();

    public async Task FlushAsync()
    {
        await Task.CompletedTask;
    }
}
