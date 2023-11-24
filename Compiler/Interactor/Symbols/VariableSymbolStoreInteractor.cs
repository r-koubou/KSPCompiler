using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.UseCases.Symbols;
using KSPCompiler.UseCases.Symbols.Commons;

namespace KSPCompiler.Interactor.Symbols;

public class VariableSymbolStoreInteractor : IVariableSymbolStoreUseCase
{
    private readonly IExternalVariableSymbolExporter exporter;

    public VariableSymbolStoreInteractor( IExternalVariableSymbolExporter exporter )
    {
        this.exporter = exporter;
    }

    public async Task<Unit> ExecuteAsync( ISymbolTable<VariableSymbol> table, CancellationToken cancellationToken = default )
    {
        await exporter.ExportAsync( table, cancellationToken );
        return Unit.Default;
    }
}
