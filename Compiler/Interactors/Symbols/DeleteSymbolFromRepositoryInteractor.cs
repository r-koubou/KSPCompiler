using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.Repositories;
using KSPCompiler.UseCases.Symbols;

namespace KSPCompiler.Interactors.Symbols;

public class DeleteSymbolFromRepositoryInteractor<TSymbol> : IDeleteSymbolUseCase<TSymbol> where TSymbol : SymbolBase
{
    private ISymbolRepository<TSymbol> Repository { get; }

    public DeleteSymbolFromRepositoryInteractor( ISymbolRepository<TSymbol> repository )
    {
        Repository = repository;
    }

    public async Task<DeleteOutputData> ExecuteAsync( DeleteSymbolInputData<TSymbol> parameter, CancellationToken cancellationToken = default )
    {
        var symbols = await Repository.FindAsync( parameter.InputData, cancellationToken );
        var deleteResult = await Repository.DeleteAsync( symbols, cancellationToken );

        return new DeleteOutputData(
            deleteResult.Success,
            deleteResult.Exception,
            new DeleteOutputDetail( deleteResult.DeletedCount
            )
        );
    }
}
