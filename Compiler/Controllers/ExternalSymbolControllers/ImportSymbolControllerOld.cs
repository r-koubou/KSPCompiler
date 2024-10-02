using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Domain.Symbols;
using KSPCompiler.UseCases;
using KSPCompiler.UseCases.Symbols;

namespace KSPCompiler.ExternalSymbolControllers;

public class ImportSymbolControllerOld<TSymbol> where TSymbol : SymbolBase
{
    private readonly IImportSymbolUseCaseOld<TSymbol> useCase;

    public ImportSymbolControllerOld( IImportSymbolUseCaseOld<TSymbol> useCase )
    {
        this.useCase = useCase;
    }

    public ImportSymbolOutputPortOld<TSymbol> Import()
        => ImportAsync().GetAwaiter().GetResult();

    public async Task<ImportSymbolOutputPortOld<TSymbol>> ImportAsync( CancellationToken cancellationToken = default )
    {
        return await useCase.ExecuteAsync( UnitInputPort.Default, cancellationToken );
    }
}
