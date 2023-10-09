using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.UseCases.Symbols;

public interface IExternalSymbolConvertUseCase
{
    void Convert<TSymbol>( ISymbolRepository<TSymbol> source, ISymbolRepository<TSymbol> target ) where TSymbol : SymbolBase;
}
