using KSPCompiler.Domain.Symbols;
using KSPCompiler.UseCases.Symbols;

namespace KSPCompiler.Interactor.Symbols;

public class NewDatabaseCreateInteractor : INewDatabaseCreateUseCase
{
    public void Create( IVariableSymbolRepository repository )
    {
        var table = new VariableSymbolTable();
        var example = new VariableSymbol
        {
            Name = "$example1",
            Description = "example1 symbol",
            Reserved = true,
        };


        table.Add( example );

        repository.Store( table );
    }
}
