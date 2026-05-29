using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.SymbolManagement.Gateways;
using KSPCompiler.Features.SymbolManagement.UseCase.Abstractions;
using KSPCompiler.Shared;
using KSPCompiler.Shared.Domain.Compilation.Symbols;

namespace KSPCompiler.Features.SymbolManagement.UseCase;

public class DeleteSymbolFromRepositoryInteractor<TSymbol> : IDeleteSymbolUseCase<TSymbol> where TSymbol : SymbolBase
{
    private ISymbolRepository<TSymbol> Repository { get; }

    public DeleteSymbolFromRepositoryInteractor( ISymbolRepository<TSymbol> repository )
    {
        Repository = repository;
    }

    public async Task<Result<DeleteOutput, SymbolManagementFailureReason>> ExecuteAsync( DeleteSymbolInput<TSymbol> input, CancellationToken cancellationToken = default )
    {
        var symbols = await Repository.FindAsync( input.Input, cancellationToken );
        var deleteResult = await Repository.DeleteAsync( symbols, cancellationToken );

        return Result<DeleteOutput, SymbolManagementFailureReason>.Success(
            new DeleteOutput(
                deleteResult.DeletedCount,
                deleteResult.FailedCount
            )
        );
    }
}
