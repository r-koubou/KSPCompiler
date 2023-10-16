using KSPCompiler.Domain.Symbols;
using KSPCompiler.UseCases.Symbols;

namespace KSPCompiler.Interactor.Symbols;

public class ExternalVariableSymbolConvertInteractor : IExternalVariableSymbolConvertUseCase
{
    public void Convert( ISymbolRepository<VariableSymbol> source, ISymbolRepository<VariableSymbol> target )
    {
        var sourceTable = source.TryLoad( () => new VariableSymbolTable() );
        var targetTable = target.TryLoad( () => new VariableSymbolTable() );

        targetTable.Merge( sourceTable );
        target.Store( targetTable );
    }
}
