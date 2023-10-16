using System;

namespace KSPCompiler.Domain.Symbols;

public interface ISymbolRepository<TSymbol> : IDisposable where TSymbol : SymbolBase
{
    /// <summary>
    /// Load symbol table from repository.
    /// </summary>
    /// <returns>
    /// A symbol table when success, otherwise throw exception.
    /// </returns>
    ISymbolTable<TSymbol> Load();

    /// <summary>
    /// Save symbol table to repository.
    /// </summary>
    void Store( ISymbolTable<TSymbol> store );

    /// <summary>
    /// Load symbol table from repository with try-catch scope.
    /// </summary>
    /// <returns>
    /// A symbol table when success, otherwise return `defaultSymbolTable` behavior.
    /// </returns>
    ISymbolTable<TSymbol> TryLoad<TSymbolTable>( Func<TSymbolTable> defaultSymbolTable ) where TSymbolTable : ISymbolTable<TSymbol>
    {
        try
        {
            return Load();
        }
        catch( Exception e )
        {
            return defaultSymbolTable();
        }
    }
}

public interface IVariableSymbolRepository : ISymbolRepository<VariableSymbol> {}

public interface ICommandSymbolRepository : ISymbolRepository<CommandSymbol> {}
