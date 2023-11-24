using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Domain.Symbols;
using KSPCompiler.UseCases.Symbols;
using KSPCompiler.UseCases.Symbols.Commons;

namespace KSPCompiler.Interactor.Symbols;

public class NewDatabaseCreateInteractor : INewDatabaseCreateUseCase
{
    private IExternalVariableSymbolExporter Exporter { get; }

    public NewDatabaseCreateInteractor( IExternalVariableSymbolExporter exporter )
    {
        Exporter = exporter;
    }

    public async Task ExecuteAsync( CancellationToken cancellationToken = default )
    {
        var table = new VariableSymbolTable();
        var example = new VariableSymbol
        {
            Name        = "$example1",
            Description = "example1 symbol",
            Reserved    = true,
        };


        table.Add( example );
        await Exporter.ExportAsync( table, cancellationToken );
    }
}
