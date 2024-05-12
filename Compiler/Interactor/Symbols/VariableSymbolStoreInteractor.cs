using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.UseCases.Symbols;

namespace KSPCompiler.Interactor.Symbols;

public class VariableSymbolStoreInteractor : IVariableSymbolStoreUseCase
{
    private readonly ISymbolExporter<VariableSymbol> exporter;

    public VariableSymbolStoreInteractor( ISymbolExporter<VariableSymbol> exporter )
    {
        this.exporter = exporter;
    }

    public async Task<Unit> ExecuteAsync( IEnumerable<VariableSymbol> symbols, CancellationToken cancellationToken = default )
    {
        await exporter.ExportAsync( symbols, cancellationToken );
        return Unit.Default;
    }
}
