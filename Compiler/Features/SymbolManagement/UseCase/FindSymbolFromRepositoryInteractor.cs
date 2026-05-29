using System;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.SymbolManagement.Gateways;
using KSPCompiler.Features.SymbolManagement.UseCase.Abstractions;
using KSPCompiler.Shared;
using KSPCompiler.Shared.Domain.Compilation.Symbols;

namespace KSPCompiler.Features.SymbolManagement.UseCase;

public class FindSymbolFromRepositoryInteractor<TSymbol> : IFindSymbolUseCase<TSymbol> where TSymbol : SymbolBase
{
    private ISymbolRepository<TSymbol> Repository { get; }

    public FindSymbolFromRepositoryInteractor( ISymbolRepository<TSymbol> repository )
    {
        Repository = repository;
    }

    public async Task<Result<FindSymbolOutput<TSymbol>, SymbolManagementFailureReason>> ExecuteAsync( FindSymbolInput<TSymbol> input, CancellationToken cancellationToken = default )
    {
        try
        {
            var predicate = input.Input;
            var symbols = await Repository.FindAsync( predicate, cancellationToken );

            return Result<FindSymbolOutput<TSymbol>, SymbolManagementFailureReason>.Success( new FindSymbolOutput<TSymbol>( symbols ) );
        }
        catch( Exception e )
        {
            return Result<FindSymbolOutput<TSymbol>, SymbolManagementFailureReason>.Failure( SymbolManagementFailureReason.Other, e );
        }
    }
}
