using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Domain.Symbols;
using KSPCompiler.UseCases;
using KSPCompiler.UseCases.Symbols;

namespace KSPCompiler.ExternalSymbolControllers;

public class ImportSymbolController<TSymbol> where TSymbol : SymbolBase
{
    private readonly IImportSymbolUseCase<TSymbol> useCase;

    public ImportSymbolController( IImportSymbolUseCase<TSymbol> useCase )
    {
        this.useCase = useCase;
    }

    public ImportSymbolOutputPort<TSymbol> Import()
        => ImportAsync().GetAwaiter().GetResult();

    public async Task<ImportSymbolOutputPort<TSymbol>> ImportAsync( CancellationToken cancellationToken = default )
    {
        return await useCase.ExecuteAsync( UnitInputPort.Default, cancellationToken );
    }
}
