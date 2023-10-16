using KSPCompiler.Domain.Symbols;
using KSPCompiler.UseCases.Symbols;

namespace KSPCompiler.Interactor.Symbols;

public class VariableSymbolStoreInteractor : IVariableSymbolStoreUseCase
{
    private readonly IVariableSymbolRepository repository;

    public VariableSymbolStoreInteractor( IVariableSymbolRepository repository )
    {
        this.repository = repository;
    }

    public void Execute( ISymbolTable<VariableSymbol> table )
    {
        repository.Store( table );
    }
}
