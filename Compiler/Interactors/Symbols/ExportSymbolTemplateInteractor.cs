using System;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Domain.Symbols;
using KSPCompiler.UseCases;
using KSPCompiler.UseCases.Symbols;

namespace KSPCompiler.Interactors.Symbols;

public class ExportSymbolTemplateInteractor<TSymbol>
    : IExportSymbolTemplateUseCase<TSymbol> where TSymbol : SymbolBase
{
    public async Task<UnitOutputPort> ExecuteAsync( ExportSymbolTemplateInputPort<TSymbol> parameter, CancellationToken cancellationToken = default )
    {
        try
        {
            await parameter.HandlingInputData.ExportTemplateAsync( cancellationToken );

            return new UnitOutputPort( true );
        }
        catch( Exception e )
        {
            return new UnitOutputPort( false, e );
        }
    }
}
