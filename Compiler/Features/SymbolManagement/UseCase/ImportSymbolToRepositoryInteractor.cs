using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.SymbolManagement.Gateways;
using KSPCompiler.Features.SymbolManagement.UseCase.Abstractions;
using KSPCompiler.Shared;
using KSPCompiler.Shared.Domain.Compilation.Symbols;

namespace KSPCompiler.Features.SymbolManagement.UseCase;

public class ImportSymbolToRepositoryInteractor<TSymbol>(
    ISymbolRepository<TSymbol> repository
) : IImportSymbolUseCase<TSymbol> where TSymbol : SymbolBase
{
    private ISymbolRepository<TSymbol> Repository { get; } = repository;

    public async Task<Result<ImportSymbolOutput, SymbolManagementFailureReason>> ExecuteAsync( ImportSymbolInput<TSymbol> input, CancellationToken cancellationToken = default )
    {
        var symbols  = await input.Importer.ImportAsync( cancellationToken );
        var storeResult = await Repository.StoreAsync( symbols, cancellationToken );

        return Result<ImportSymbolOutput, SymbolManagementFailureReason>.Success(
            new ImportSymbolOutput(
                storeResult.CreatedCount,
                storeResult.UpdatedCount,
                storeResult.FailedCount
            )
        );
    }
}
