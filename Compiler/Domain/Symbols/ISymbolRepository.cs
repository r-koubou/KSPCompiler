using System;
using System.Threading;
using System.Threading.Tasks;

namespace KSPCompiler.Domain.Symbols;

public interface ISymbolRepository<TSymbol> : IDisposable where TSymbol : SymbolBase
{
    /// <summary>
    /// Load symbol table from repository.
    /// </summary>
    /// <returns>
    /// A symbol table when success, otherwise throw exception.
    /// </returns>
    public ISymbolTable<TSymbol> Load()
        => LoadAsync().GetAwaiter().GetResult();

    /// <summary>
    /// Load symbol table from repository asynchronously.
    /// </summary>
    Task<ISymbolTable<TSymbol>> LoadAsync( CancellationToken cancellationToken = default );

    /// <summary>
    /// Save symbol table to repository.
    /// </summary>
    void Store( ISymbolTable<TSymbol> store )
        => StoreAsync( store ).GetAwaiter().GetResult();

    /// <summary>
    /// Save symbol table to repository.
    /// </summary>
    Task StoreAsync( ISymbolTable<TSymbol> store, CancellationToken cancellationToken = default );

    /// <summary>
    /// Load symbol table from repository with try-catch scope.
    /// </summary>
    /// <returns>
    /// A symbol table when success, otherwise return `defaultSymbolTable` behavior.
    /// </returns>
    (bool reeult, ISymbolTable<TSymbol> table, Exception? error ) TryLoad<TSymbolTable>( Func<TSymbolTable> defaultSymbolTable ) where TSymbolTable : ISymbolTable<TSymbol>
        => TryLoadAsync( defaultSymbolTable ).GetAwaiter().GetResult();

    /// <summary>
    /// Load symbol table from repository with try-catch scope asynchronously.
    /// </summary>
    /// <returns>
    /// A symbol table when success, otherwise return `defaultSymbolTable` behavior.
    /// </returns>
    async Task<(bool reeult, ISymbolTable<TSymbol> table, Exception? error )> TryLoadAsync<TSymbolTable>(
        Func<TSymbolTable> defaultSymbolTable,
        CancellationToken cancellationToken = default )
        where TSymbolTable : ISymbolTable<TSymbol>
    {
        try
        {
            return ( true, await LoadAsync( cancellationToken ), null );
        }
        catch( Exception e )
        {
            return ( false, defaultSymbolTable.Invoke(), e );
        }
    }
}
