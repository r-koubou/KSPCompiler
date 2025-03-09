using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.SymbolManagement.Gateways;
using KSPCompiler.Features.SymbolManagement.UseCase.Abstractions;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Abstractions.Symbol;

namespace KSPCompiler.Features.SymbolManagement.UseCase.ApplicationServices;

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
