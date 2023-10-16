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
    {
        useCase.Execute( table );
    }
}
