using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Domain.Symbols;
using KSPCompiler.UseCases.Symbols;

namespace KSPCompiler.Interactor.Symbols;

public class NewDatabaseCreateInteractor : INewDatabaseCreateUseCase
{
    private ISymbolExporter<VariableSymbol> Exporter { get; }

    public NewDatabaseCreateInteractor( ISymbolExporter<VariableSymbol> exporter )
    {
        Exporter = exporter;
    }

    public async Task ExecuteAsync( CancellationToken cancellationToken = default )
    {
        var list = new List<VariableSymbol>
        {
            new()
            {
                Name        = "$example1",
                Description = "example1 symbol",
                Reserved    = true,
            }
        };

        await Exporter.ExportAsync( list, cancellationToken );
    }
}
