using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.UseCases.Symbols;

public interface IVariableSymbolStoreUseCase
{
    void Execute( ISymbolTable<VariableSymbol> table );
}
