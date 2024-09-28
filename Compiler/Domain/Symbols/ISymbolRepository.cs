using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KSPCompiler.Domain.Symbols;

public interface ISymbolRepository<TSymbol> : IDisposable where TSymbol : SymbolBase
{
    public static readonly Func<TSymbol, bool> Any = x => true;

    public bool Store( TSymbol symbol )
        => StoreAsync( symbol ).GetAwaiter().GetResult();

    public Task<bool> StoreAsync( TSymbol symbol );

    public bool Store( IEnumerable<TSymbol> symbols )
        => StoreAsync( symbols ).GetAwaiter().GetResult();

    public Task<bool> StoreAsync( IEnumerable<TSymbol> symbols );

    public bool Delete( TSymbol symbol )
        => DeleteAsync( symbol ).GetAwaiter().GetResult();

    public Task<bool> DeleteAsync( TSymbol symbol );

    public bool Delete( IEnumerable<TSymbol> symbols )
        => DeleteAsync( symbols ).GetAwaiter().GetResult();

    public Task<bool> DeleteAsync( IEnumerable<TSymbol> symbols );

    public IEnumerable<TSymbol> Find( Func<TSymbol, bool> predicate )
        => FindAsync( predicate ).GetAwaiter().GetResult();

    public Task<IEnumerable<TSymbol>> FindAsync( Func<TSymbol, bool> predicate, CancellationToken cancellationToken = default );
}
