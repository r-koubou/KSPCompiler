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
        var input = new ImportSymbolInput<TSymbol>( importer );
        var result = await useCase.ExecuteAsync( input, cancellationToken );

        if( result.IsFailure )
        {
            return new ImportResult( false, 0, 0, 0, result.UnwrapError().Error );
        }

        var output = result.Unwrap();

        return new ImportResult(
            true,
            output.CreatedCount,
            output.UpdatedCount,
            output.FailedCount
        );
    }

    public async Task<ExportResult> ExportAsync( ISymbolExporter<TSymbol> exporter, Predicate<TSymbol> predicate, CancellationToken cancellationToken = default )
    {
        var useCase = new ExportSymbolFromRepositoryInteractor<TSymbol>( Repository );
        var input = new ExportSymbolInput<TSymbol>(
            exporter,
            predicate
        );

        var result = await useCase.ExecuteAsync( input, cancellationToken );

        return result.IsFailure
            ? new ExportResult( false, result.UnwrapError().Error )
            : new ExportResult( true );
    }

    public async Task<DeleteResult> DeleteAsync( Predicate<TSymbol> predicate, CancellationToken cancellationToken = default )
    {
        var useCase = new DeleteSymbolFromRepositoryInteractor<TSymbol>( Repository );
        var input = new DeleteSymbolInput<TSymbol>( predicate );
        var result = await useCase.ExecuteAsync( input, cancellationToken );

        if( result.IsFailure )
        {
            return new DeleteResult( false, 0, 0, result.UnwrapError().Error );
        }

        var output = result.Unwrap();

        return new DeleteResult(
            success: true,
            deletedCount: output.DeletedCount,
            failedCount: output.FailedCount
        );
    }

    public async Task<FindResult<TSymbol>> FindAsync( Predicate<TSymbol> predicate, CancellationToken cancellationToken = default )
    {
        var useCase = new FindSymbolFromRepositoryInteractor<TSymbol>( Repository );
        var input = new FindSymbolInput<TSymbol>( predicate );
        var result = await useCase.ExecuteAsync( input, cancellationToken );

        if( result.IsFailure )
        {
            return new FindResult<TSymbol>(
                false,
                [ ],
                result.UnwrapError().Error
            );
        }

        return new FindResult<TSymbol>( true, result.Unwrap().Symbols );
    }
}
