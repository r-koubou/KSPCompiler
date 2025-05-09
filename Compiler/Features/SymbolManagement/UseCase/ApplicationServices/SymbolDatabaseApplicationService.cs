using System;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.SymbolManagement.Gateways;
using KSPCompiler.Features.SymbolManagement.UseCase.Abstractions;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Abstractions.Symbol;

namespace KSPCompiler.Features.SymbolManagement.UseCase.ApplicationServices;

public sealed class SymbolDatabaseApplicationService<TSymbol>( ISymbolRepository<TSymbol> repository )
    where TSymbol : SymbolBase
{
    private ISymbolRepository<TSymbol> Repository { get; } = repository;

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
            success: outputPort.Result,
            deletedCount: outputPort.OutputData.DeletedCount,
            failedCount: outputPort.OutputData.FailedCount,
            exception: outputPort.Error
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
