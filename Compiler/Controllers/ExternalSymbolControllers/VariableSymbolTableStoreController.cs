using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Domain.Symbols;
using KSPCompiler.UseCases.Symbols;

namespace KSPCompiler.ExternalSymbolControllers;

public class VariableSymbolTableStoreController
{
    private readonly IVariableSymbolStoreUseCase useCase;

    public VariableSymbolTableStoreController( IVariableSymbolStoreUseCase useCase )
    {
        this.useCase = useCase;
    }

    public void Store( IEnumerable<VariableSymbol> symbols )
        => StoreAsync( symbols ).GetAwaiter().GetResult();

    public async Task StoreAsync( IEnumerable<VariableSymbol> symbols, CancellationToken cancellationToken = default )
    {
        await useCase.ExecuteAsync( symbols, cancellationToken );
    }
}
