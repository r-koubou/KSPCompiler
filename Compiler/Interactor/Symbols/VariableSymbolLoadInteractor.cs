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

    public VariableSymbolLoadOutputData Execute()
    {
        var (result, table, error) = repository.TryLoad( () => new VariableSymbolTable() );

        return new VariableSymbolLoadOutputData( result, table, error );
    }
}
