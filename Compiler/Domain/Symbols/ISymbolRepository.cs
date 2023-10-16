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
    (bool reeult, ISymbolTable<TSymbol> table, Exception? error ) TryLoad<TSymbolTable>( Func<TSymbolTable> defaultSymbolTable ) where TSymbolTable : ISymbolTable<TSymbol>
    {
        try
        {
            return ( true, Load(), null );
        }
        catch( Exception e )
        {
            return ( false, defaultSymbolTable.Invoke(), e );
        }
    }
}
