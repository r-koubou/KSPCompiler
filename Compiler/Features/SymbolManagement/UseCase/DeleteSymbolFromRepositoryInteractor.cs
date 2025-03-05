using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.SymbolManagement.Gateways;
using KSPCompiler.Features.SymbolManagement.UseCase.Abstractions;
using KSPCompiler.Shared.Domain.Symbols;

namespace KSPCompiler.Features.SymbolManagement.UseCase;

public class DeleteSymbolFromRepositoryInteractor<TSymbol> : IDeleteSymbolUseCase<TSymbol> where TSymbol : SymbolBase
{
    private ISymbolRepository<TSymbol> Repository { get; }

    public DeleteSymbolFromRepositoryInteractor( ISymbolRepository<TSymbol> repository )
    {
        Repository = repository;
    }

    public async Task<DeleteOutputData> ExecuteAsync( DeleteSymbolInputData<TSymbol> parameter, CancellationToken cancellationToken = default )
    {
        var symbols = await Repository.FindAsync( parameter.Input, cancellationToken );
        var deleteResult = await Repository.DeleteAsync( symbols, cancellationToken );

        return new DeleteOutputData( new DeleteOutputDetail( deleteResult.DeletedCount
                                     ), deleteResult.Success, deleteResult.Exception
        );
    }
}
