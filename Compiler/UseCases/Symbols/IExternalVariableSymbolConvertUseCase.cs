using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.UseCases.Symbols;

public interface IExternalVariableSymbolConvertUseCase
{
    void Convert( ISymbolRepository<VariableSymbol> source, ISymbolRepository<VariableSymbol> target );
}
