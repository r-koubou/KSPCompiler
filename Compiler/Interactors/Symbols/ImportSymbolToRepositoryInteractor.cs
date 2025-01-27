using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Domain.Symbols;
using KSPCompiler.Gateways.Symbols;
using KSPCompiler.UseCases.Symbols;

namespace KSPCompiler.Interactors.Symbols;

public class ImportSymbolToRepositoryInteractor<TSymbol> : IImportSymbolUseCase<TSymbol> where TSymbol : SymbolBase
{
    private ISymbolRepository<TSymbol> Repository { get; }

    public ImportSymbolToRepositoryInteractor( ISymbolRepository<TSymbol> repository )
    {
        Repository = repository;
    }

    public async Task<ImportSymbolOutputPort> ExecuteAsync( ImportSymbolInputPort<TSymbol> parameter, CancellationToken cancellationToken = default )
    {
        var symbols  = await parameter.InputData.ImportAsync( cancellationToken );
        var storeResult = await Repository.StoreAsync( symbols, cancellationToken );

        var detail = new ImportSymbolOutputPortDetail(
            storeResult.CreatedCount,
            storeResult.UpdatedCount,
            storeResult.FailedCount
        );

        return new ImportSymbolOutputPort(
            storeResult.Success,
            detail,
            storeResult.Exception
        );
    }
}
