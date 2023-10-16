using KSPCompiler.Domain.Symbols;
using KSPCompiler.UseCases.Symbols;

namespace KSPCompiler.ExternalSymbolControllers;

public class ExternalVariableSymbolTableLoadController
{
    private readonly IVariableSymbolLoadUseCase useCase;

    public ExternalVariableSymbolTableLoadController( IVariableSymbolLoadUseCase useCase )
    {
        this.useCase = useCase;
    }

    public ISymbolTable<VariableSymbol> Load()
    {
        return useCase.Execute();
    }
}
