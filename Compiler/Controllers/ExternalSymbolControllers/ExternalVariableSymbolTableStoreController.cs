using KSPCompiler.Domain.Symbols;
using KSPCompiler.UseCases.Symbols;

namespace KSPCompiler.ExternalSymbolControllers;

public class ExternalVariableSymbolTableStoreController
{
    private readonly IVariableSymbolStoreUseCase useCase;

    public ExternalVariableSymbolTableStoreController( IVariableSymbolStoreUseCase useCase )
    {
        this.useCase = useCase;
    }

    public void Store( ISymbolTable<VariableSymbol> table )
    {
        useCase.Execute( table );
    }
}
