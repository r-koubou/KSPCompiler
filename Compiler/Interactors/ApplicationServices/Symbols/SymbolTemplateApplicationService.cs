using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Domain.Symbols;
using KSPCompiler.Gateways.Symbols;
using KSPCompiler.Interactors.Symbols;
using KSPCompiler.UseCases.Symbols;

namespace KSPCompiler.Interactors.ApplicationServices.Symbols;

public sealed class SymbolTemplateApplicationService<TSymbol> where TSymbol : SymbolBase
{
    public async Task<ExportResult> ExportAsync( ISymbolExporter<TSymbol> exporter, CancellationToken cancellationToken = default )
    {
        var useCase = new ExportSymbolTemplateInteractor<TSymbol>();
        var inputPort = new ExportSymbolTemplateInputPort<TSymbol>( exporter );
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
}
