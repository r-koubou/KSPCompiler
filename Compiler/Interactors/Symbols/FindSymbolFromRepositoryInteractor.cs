using System;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Domain.Symbols;
using KSPCompiler.Gateways.Symbols;
using KSPCompiler.UseCases.Symbols;

namespace KSPCompiler.Interactors.Symbols;

public class FindSymbolFromRepositoryInteractor<TSymbol> : IFindSymbolUseCase<TSymbol> where TSymbol : SymbolBase
{
    private ISymbolRepository<TSymbol> Repository { get; }

    public FindSymbolFromRepositoryInteractor( ISymbolRepository<TSymbol> repository )
    {
        Repository = repository;
    }

    public async Task<FindSymbolOutputData<TSymbol>> ExecuteAsync( FindSymbolInputData<TSymbol> parameter, CancellationToken cancellationToken = default )
    {
        try
        {
            var predicate = parameter.Data;
            var symbols = await Repository.FindAsync( predicate, cancellationToken );

            return new FindSymbolOutputData<TSymbol>( symbols, true );
        }
        catch( Exception e )
        {
            return new FindSymbolOutputData<TSymbol>( Array.Empty<TSymbol>(), false, e );
        }
    }
}
