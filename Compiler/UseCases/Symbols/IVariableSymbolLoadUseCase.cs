using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.UseCases.Symbols;

public interface IVariableSymbolLoadUseCase
{
    ISymbolTable<VariableSymbol> Execute();
}
