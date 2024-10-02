using System;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Domain.Symbols;
using KSPCompiler.UseCases;
using KSPCompiler.UseCases.Symbols;

namespace KSPCompiler.Interactor.Symbols;

public class ExportSymbolInteractorOld<TSymbol> : IExportSymbolUseCaseOld<TSymbol> where TSymbol : SymbolBase
{
    private readonly ISymbolExporter<TSymbol> exporter;

    public ExportSymbolInteractorOld( ISymbolExporter<TSymbol> exporter )
    {
        this.exporter = exporter;
    }

    public async Task<UnitOutputPort> ExecuteAsync( ExportSymbolInputDataOld<TSymbol> parameter, CancellationToken cancellationToken = default )
    {
        try
        {
            await exporter.ExportAsync( parameter.InputData, cancellationToken );
            return new UnitOutputPort( true );
        }
        catch( Exception e )
        {
            return new UnitOutputPort( false, e );
        }
    }
}
