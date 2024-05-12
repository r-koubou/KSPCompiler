using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Domain.Symbols;
using KSPCompiler.UseCases.Symbols;

namespace KSPCompiler.ExternalSymbolControllers;

public class SymbolStoreController<TSymbol> where TSymbol : SymbolBase
{
    private readonly ISymbolStoreUseCase<TSymbol> useCase;

    public SymbolStoreController( ISymbolStoreUseCase<TSymbol> useCase )
    {
        this.useCase = useCase;
    }

    public void Store( IEnumerable<TSymbol> symbols )
        => StoreAsync( symbols ).GetAwaiter().GetResult();

    public async Task StoreAsync( IEnumerable<TSymbol> symbols, CancellationToken cancellationToken = default )
    {
        await useCase.ExecuteAsync( symbols, cancellationToken );
    }
}
