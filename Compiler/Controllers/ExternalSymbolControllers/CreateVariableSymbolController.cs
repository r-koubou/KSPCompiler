using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.UseCases.Symbols;

namespace KSPCompiler.ExternalSymbolControllers;

public class CreateVariableSymbolController
{
    private readonly ICreateVariableSymbolUseCaseOld useCase;

    public CreateVariableSymbolController( ICreateVariableSymbolUseCaseOld useCase )
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
