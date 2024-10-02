using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Domain.Symbols;
using KSPCompiler.UseCases.Symbols;

namespace KSPCompiler.ExternalSymbolControllers;

public class ExportSymbolControllerOld<TSymbol> where TSymbol : SymbolBase
{
    private readonly IExportSymbolUseCaseOld<TSymbol> useCase;

    public ExportSymbolControllerOld( IExportSymbolUseCaseOld<TSymbol> useCase )
    {
        this.useCase = useCase;
    }

    public void Export( ExportSymbolInputDataOld<TSymbol> symbols )
        => ExportAsync( symbols ).GetAwaiter().GetResult();

    public async Task ExportAsync( ExportSymbolInputDataOld<TSymbol> symbols, CancellationToken cancellationToken = default )
    {
        await useCase.ExecuteAsync( symbols, cancellationToken );
    }
}
