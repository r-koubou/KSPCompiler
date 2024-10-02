using System;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.UseCases.Symbols;

namespace KSPCompiler.ExternalSymbolControllers;

[Obsolete]
public class CreateVariableSymbolControllerOld
{
    private readonly ICreateVariableSymbolUseCaseOld useCase;

    public CreateVariableSymbolControllerOld( ICreateVariableSymbolUseCaseOld useCase )
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
