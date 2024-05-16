using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Domain.Symbols;
using KSPCompiler.UseCases.Symbols;

namespace KSPCompiler.ExternalSymbolControllers;

public class ExportSymbolController<TSymbol> where TSymbol : SymbolBase
{
    private readonly IExportSymbolUseCase<TSymbol> useCase;

    public ExportSymbolController( IExportSymbolUseCase<TSymbol> useCase )
    {
        this.useCase = useCase;
    }

    public void Export( ExportSymbolInputData<TSymbol> symbols )
        => ExportAsync( symbols ).GetAwaiter().GetResult();

    public async Task ExportAsync( ExportSymbolInputData<TSymbol> symbols, CancellationToken cancellationToken = default )
    {
        await useCase.ExecuteAsync( symbols, cancellationToken );
    }
}
