using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Shared.Domain.Compilation.Symbols;

namespace KSPCompiler.Features.Compilation.Gateways.Symbols;

public interface ISymbolRepository<TSymbol> : IDisposable where TSymbol : SymbolBase
{
    public int Count { get; }

    public IReadOnlyCollection<TSymbol> FindByName( string name )
        => FindByNameAsync( name ).GetAwaiter().GetResult();

    public Task<IReadOnlyCollection<TSymbol>>  FindByNameAsync( string name, CancellationToken cancellationToken = default );

    public IReadOnlyCollection<TSymbol> Find( Predicate<TSymbol> predicate )
        => FindAsync( predicate ).GetAwaiter().GetResult();

    public Task<IReadOnlyCollection<TSymbol>> FindAsync( Predicate<TSymbol> predicate, CancellationToken cancellationToken = default );

    public IReadOnlyCollection<TSymbol> FindAll()
        => FindAllAsync().GetAwaiter().GetResult();

    public Task<IReadOnlyCollection<TSymbol>> FindAllAsync( CancellationToken cancellationToken = default );
}
