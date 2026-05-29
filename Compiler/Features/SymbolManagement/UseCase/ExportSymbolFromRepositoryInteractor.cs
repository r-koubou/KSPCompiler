using System;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.SymbolManagement.Gateways;
using KSPCompiler.Features.SymbolManagement.UseCase.Abstractions;
using KSPCompiler.Shared;
using KSPCompiler.Shared.Domain.Compilation.Symbols;

namespace KSPCompiler.Features.SymbolManagement.UseCase;

public class ExportSymbolFromRepositoryInteractor<TSymbol> : IExportSymbolUseCase<TSymbol> where TSymbol : SymbolBase
{
    private ISymbolRepository<TSymbol> Repository { get; }

    public ExportSymbolFromRepositoryInteractor( ISymbolRepository<TSymbol> repository )
    {
        Repository = repository;
    }

    public async Task<Result<Unit, SymbolManagementFailureReason>> ExecuteAsync( ExportSymbolInputData<TSymbol> input, CancellationToken cancellationToken = default )
    {
        try
        {
            var exporter = input.Exporter;
            var predicate = input.Predicate;
            var symbols = await Repository.FindAsync( predicate, cancellationToken );

            await exporter.ExportAsync( symbols, cancellationToken );

            return Result<Unit, SymbolManagementFailureReason>.Success( Unit.Default );
        }
        catch( Exception e )
        {
            return Result<Unit, SymbolManagementFailureReason>.Failure( SymbolManagementFailureReason.Other, e );
        }
    }
}
