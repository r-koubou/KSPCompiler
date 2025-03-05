using System;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.SymbolManagement.Gateways;
using KSPCompiler.Features.SymbolManagement.UseCase.Abstractions;
using KSPCompiler.Shared.Domain.Symbols;
using KSPCompiler.Shared.UseCase;

namespace KSPCompiler.Features.SymbolManagement.UseCase;

public class ExportSymbolFromRepositoryInteractor<TSymbol> : IExportSymbolUseCase<TSymbol> where TSymbol : SymbolBase
{
    private ISymbolRepository<TSymbol> Repository { get; }

    public ExportSymbolFromRepositoryInteractor( ISymbolRepository<TSymbol> repository )
    {
        Repository = repository;
    }

    public async Task<UnitOutputPort> ExecuteAsync( ExportSymbolInputData<TSymbol> parameter, CancellationToken cancellationToken = default )
    {
        try
        {
            var exporter = parameter.Input.Exporter;
            var predicate = parameter.Input.Predicate;
            var symbols = await Repository.FindAsync( predicate, cancellationToken );

            await exporter.ExportAsync( symbols, cancellationToken );
            return new UnitOutputPort( true );
        }
        catch( Exception e )
        {
            return new UnitOutputPort( false, e );
        }
    }
}
