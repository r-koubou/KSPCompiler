using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.UseCases.Symbols;

namespace KSPCompiler.Interactor.Symbols;

public class ExportSymbolInteractor<TSymbol> : IExportSymbolUseCase<TSymbol> where TSymbol : SymbolBase
{
    private readonly ISymbolExporter<TSymbol> exporter;

    public ExportSymbolInteractor( ISymbolExporter<TSymbol> exporter )
    {
        this.exporter = exporter;
    }

    public async Task<Unit> ExecuteAsync( IEnumerable<TSymbol> symbols, CancellationToken cancellationToken = default )
    {
        await exporter.ExportAsync( symbols, cancellationToken );
        return Unit.Default;
    }
}
