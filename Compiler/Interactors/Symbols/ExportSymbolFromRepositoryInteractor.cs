using System;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.Repositories;
using KSPCompiler.UseCases;
using KSPCompiler.UseCases.Symbols;

namespace KSPCompiler.Interactors.Symbols;

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
            var exporter = parameter.InputData.Exporter;
            var predicate = parameter.InputData.Predicate;
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
