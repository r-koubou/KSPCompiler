using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.SymbolManagement.Gateways;
using KSPCompiler.Features.SymbolManagement.UseCase.Abstractions;
using KSPCompiler.Shared.Domain.Compilation.Symbols;

namespace KSPCompiler.Features.SymbolManagement.UseCase;

public class ImportSymbolToRepositoryInteractor<TSymbol>(
    ISymbolRepository<TSymbol> repository
) : IImportSymbolUseCase<TSymbol> where TSymbol : SymbolBase
{
    private ISymbolRepository<TSymbol> Repository { get; } = repository;

    public async Task<ImportSymbolOutputPort> ExecuteAsync( ImportSymbolInputPort<TSymbol> parameter, CancellationToken cancellationToken = default )
    {
        var symbols  = await parameter.Input.ImportAsync( cancellationToken );
        var storeResult = await Repository.StoreAsync( symbols, cancellationToken );

        var detail = new ImportSymbolOutputPortDetail(
            storeResult.CreatedCount,
            storeResult.UpdatedCount,
            storeResult.FailedCount
        );

        return new ImportSymbolOutputPort(
            detail,
            storeResult.Success,
            storeResult.Exception
        );
    }
}
