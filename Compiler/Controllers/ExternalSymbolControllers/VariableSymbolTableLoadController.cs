using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Commons;
using KSPCompiler.UseCases.Symbols;

namespace KSPCompiler.ExternalSymbolControllers;

public class VariableSymbolTableLoadController
{
    private readonly IVariableSymbolLoadUseCase useCase;

    public VariableSymbolTableLoadController( IVariableSymbolLoadUseCase useCase )
    {
        this.useCase = useCase;
    }

    public VariableSymbolLoadOutputData Load()
        => LoadAsync().GetAwaiter().GetResult();

    public async Task<VariableSymbolLoadOutputData> LoadAsync( CancellationToken cancellationToken = default )
    {
        return await useCase.ExecuteAsync(Unit.Default, cancellationToken );
    }
}
