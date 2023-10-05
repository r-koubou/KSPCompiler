using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Gateways;

public interface IExternalSymbolRepository<TLoadSymbol, in TStore> where TLoadSymbol : SymbolBase
{
    ISymbolTable<TLoadSymbol> LoadSymbolTable();
    void StoreSymbolTable( TStore store );
}
