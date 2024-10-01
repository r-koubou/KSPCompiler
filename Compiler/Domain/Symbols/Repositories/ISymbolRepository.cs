using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KSPCompiler.Domain.Symbols.Repositories;

public interface ISymbolRepository<TSymbol> : IDisposable where TSymbol : SymbolBase
{
    public static readonly Func<TSymbol, bool> Any = x => true;

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

    public IEnumerable<TSymbol> FindByName( string name )
        => FindByNameAsync( name ).GetAwaiter().GetResult();

    public Task<IEnumerable<TSymbol>>  FindByNameAsync( string name, CancellationToken cancellationToken = default );

    public IEnumerable<TSymbol> Find( Func<TSymbol, bool> predicate )
        => FindAsync( predicate ).GetAwaiter().GetResult();

    public Task<IEnumerable<TSymbol>> FindAsync( Func<TSymbol, bool> predicate, CancellationToken cancellationToken = default );

    public IEnumerable<TSymbol> FindAll()
        => FindAllAsync().GetAwaiter().GetResult();

    public Task<IEnumerable<TSymbol>> FindAllAsync( CancellationToken cancellationToken = default );
}