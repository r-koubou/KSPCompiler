using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.UseCases.Symbols;

namespace KSPCompiler.ExternalSymbolControllers;

public class VariableSymbolTableFileCreateController
{
    private readonly INewDatabaseCreateUseCase useCase;

    public VariableSymbolTableFileCreateController( INewDatabaseCreateUseCase useCase )
    {
        this.useCase = useCase;
    }

    public void Create()
        => CreateAsync().GetAwaiter().GetResult();

    public async Task CreateAsync( CancellationToken cancellationToken = default)
    {
        await useCase.ExecuteAsync( cancellationToken );
    }
}
