using System;

namespace KSPCompiler.Domain.Symbols;

public interface ISymbolRepository<TSymbol> : System.IDisposable where TSymbol : SymbolBase
{
    ISymbolTable<TSymbol> LoadSymbolTable();
    void StoreSymbolTable( ISymbolTable<TSymbol> store );
}

public interface IVariableSymbolRepository : ISymbolRepository<VariableSymbol> {}

public interface ICommandSymbolRepository : ISymbolRepository<CommandSymbol> {}
