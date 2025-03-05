using System;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.SymbolManagement.UseCase.Abstractions;
using KSPCompiler.Shared.Domain.Symbols;
using KSPCompiler.Shared.UseCase;

namespace KSPCompiler.Features.SymbolManagement.UseCase;

public class ExportSymbolTemplateInteractor<TSymbol>
    : IExportSymbolTemplateUseCase<TSymbol> where TSymbol : SymbolBase
{
    public async Task<UnitOutputPort> ExecuteAsync( ExportSymbolTemplateInputPort<TSymbol> parameter, CancellationToken cancellationToken = default )
    {
        try
        {
            await parameter.Input.ExportTemplateAsync( cancellationToken );

            return new UnitOutputPort( true );
        }
        catch( Exception e )
        {
            return new UnitOutputPort( false, e );
        }
    }
}
