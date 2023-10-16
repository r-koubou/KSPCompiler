using KSPCompiler.Domain.Symbols;
using KSPCompiler.UseCases.Symbols;

namespace KSPCompiler.Interactor.Symbols;

public class VariableSymbolLoadInteractor : IVariableSymbolLoadUseCase
{
    private readonly IVariableSymbolRepository repository;

    public VariableSymbolLoadInteractor( IVariableSymbolRepository repository )
    {
        this.repository = repository;
    }

    public ISymbolTable<VariableSymbol> Execute()
    {
        return repository.Load();
    }
}
