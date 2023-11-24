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

    public void Store( ISymbolTable<VariableSymbol> table )
        => StoreAsync( table ).GetAwaiter().GetResult();

    public async Task StoreAsync( ISymbolTable<VariableSymbol> table, CancellationToken cancellationToken = default )
    {
        await useCase.ExecuteAsync( table, cancellationToken );
    }
}
