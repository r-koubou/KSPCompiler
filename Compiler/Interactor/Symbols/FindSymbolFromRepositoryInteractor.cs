using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.Repositories;
using KSPCompiler.UseCases.Symbols;

namespace KSPCompiler.Interactor.Symbols;

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
            var predicate = parameter.InputData;
            var symbols = await Repository.FindAsync( predicate, cancellationToken );

            return new FindSymbolOutputData<TSymbol>( true, symbols );
        }
        catch( Exception e )
        {
            return new FindSymbolOutputData<TSymbol>( false, Array.Empty<TSymbol>(), e );
        }
    }
}
