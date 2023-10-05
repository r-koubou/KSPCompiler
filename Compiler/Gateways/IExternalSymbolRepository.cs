using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Gateways;

public interface IExternalSymbolRepository<TSymbol> where TSymbol : SymbolBase
{
    ISymbolTable<TSymbol> LoadSymbolTable();
    void StoreSymbolTable( ISymbolTable<TSymbol> store );
}
