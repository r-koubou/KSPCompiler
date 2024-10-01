using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.Repositories;
using KSPCompiler.Interactor.Symbols;
using KSPCompiler.UseCases;

namespace KSPCompiler.SymbolDatabaseControllers;

public class SymbolDatabaseController<TSymbol> where TSymbol : SymbolBase
{
    private ISymbolRepository<TSymbol> Repository { get; }

    public SymbolDatabaseController( ISymbolRepository<TSymbol> repository )
    {
        Repository = repository;
    }

    public async Task<ImportResult> ImportAsync( ISymbolImporter<TSymbol> importer, CancellationToken cancellationToken = default )
    {
        var useCase = new ImportSymbolInteractor<TSymbol>( importer );
        var outputPort = await useCase.ExecuteAsync( UnitInputPort.Default, cancellationToken );

        if( !outputPort.Result )
        {
            return new ImportResult( false, 0, 0, 0, outputPort.Error );
        }

        var importedSymbols = outputPort.OutputData;
        var result = await Repository.StoreAsync( importedSymbols, cancellationToken );

        return new ImportResult(
            result.Success,
            result.CreatedCount,
            result.UpdatedCount,
            result.FailedCount,
            result.Exception
        );
    }
}
