using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.UseCases.Symbols;

namespace KSPCompiler.ExternalSymbolControllers;

public class SymbolLoadController<TSymbol> where TSymbol : SymbolBase
{
    private readonly ISymbolLoadUseCase<TSymbol> useCase;

    public SymbolLoadController( ISymbolLoadUseCase<TSymbol> useCase )
    {
        this.useCase = useCase;
    }

    public SymbolLoadOutputData<TSymbol> Load()
        => LoadAsync().GetAwaiter().GetResult();

    public async Task<SymbolLoadOutputData<TSymbol>> LoadAsync( CancellationToken cancellationToken = default )
    {
        return await useCase.ExecuteAsync(Unit.Default, cancellationToken );
    }
}
