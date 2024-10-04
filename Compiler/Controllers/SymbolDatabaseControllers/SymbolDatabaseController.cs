using System;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.Repositories;
using KSPCompiler.Interactor.Symbols;
using KSPCompiler.UseCases.Symbols;

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
        var useCase = new ImportSymbolToRepositoryInteractor<TSymbol>( Repository );
        var inputPort = new ImportSymbolInputPort<TSymbol>( importer );
        var outputPort = await useCase.ExecuteAsync( inputPort, cancellationToken );

        if( !outputPort.Result )
        {
            return new ImportResult( false, 0, 0, 0, outputPort.Error );
        }

        return new ImportResult(
            outputPort.Result,
            outputPort.OutputData.CreatedCount,
            outputPort.OutputData.UpdatedCount,
            outputPort.OutputData.FailedCount,
            outputPort.Error
        );
    }

    public async Task<ExportResult> ExportAsync( ISymbolExporter<TSymbol> exporter, Predicate<TSymbol> predicate, CancellationToken cancellationToken = default )
    {
        var useCase = new ExportSymbolFromRepositoryInteractor<TSymbol>( Repository );
        var inputPort = new ExportSymbolInputData<TSymbol>(
            new ExportSymbolInputDataDetail<TSymbol>(
                exporter,
                predicate
            )
        );

        var outputPort = await useCase.ExecuteAsync( inputPort, cancellationToken );

        if( !outputPort.Result )
        {
            return new ExportResult( false, outputPort.Error );
        }

        return new ExportResult(
            outputPort.Result,
            outputPort.Error
        );
    }

    public async Task<DeleteResult> DeleteAsync( Predicate<TSymbol> predicate, CancellationToken cancellationToken = default )
    {
        var useCase = new DeleteSymbolFromRepositoryInteractor<TSymbol>( Repository );
        var inputPort = new DeleteSymbolInputData<TSymbol>( predicate );
        var outputPort = await useCase.ExecuteAsync( inputPort, cancellationToken );

        return new DeleteResult(
            outputPort.Result,
            outputPort.OutputData.DeletedCount,
            outputPort.Error
        );
    }

    public async Task<FindResult<TSymbol>> FindAsync( Predicate<TSymbol> predicate, CancellationToken cancellationToken = default )
    {
        var useCase = new FindSymbolFromRepositoryInteractor<TSymbol>( Repository );
        var inputPort = new FindSymbolInputData<TSymbol>( predicate );
        var outputPort = await useCase.ExecuteAsync( inputPort, cancellationToken );

        return new FindResult<TSymbol>(
            outputPort.Result,
            outputPort.OutputData,
            outputPort.Error
        );
    }

}
