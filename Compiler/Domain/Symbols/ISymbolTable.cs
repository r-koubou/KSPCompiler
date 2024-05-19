using System.Collections.Generic;
using System.IO;

namespace KSPCompiler.Domain.Symbols;

// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedMember.Global
public interface ISymbolTable<TSymbol> : IEnumerable<TSymbol> where TSymbol : SymbolBase
{
    /// <summary>
    /// Symbol table (read-only)
    /// </summary>
    IReadOnlyDictionary<SymbolName, TSymbol> Table { get; }

    /// <summary>
    /// Size of symbol table
    /// </summary>
    int Count { get; }

    /// <summary>
    /// Parent node to be used when local scope, such as nesting, is allowed.
    /// </summary>
    ISymbolTable<TSymbol>? Parent { get; set; }

    /// <summary>
    /// Searches whether the specified symbol name is registered in the table
    /// </summary>
    /// <returns>Valid instance, null if not found</returns>
    bool TrySearchByName( SymbolName name, out TSymbol result, bool enableSearchParent = true );

    /// <summary>
    /// Searches whether the specified symbol index is registered in the table
    /// </summary>
    bool TrySearchByIndex( UniqueSymbolIndex index, out TSymbol result, bool enableSearchParent = true );

    /// <summary>
    /// Searches whether the specified symbol name and index is registered in the table
    /// </summary>
    bool TrySearchIndexByName( SymbolName name, out UniqueSymbolIndex result, bool enableSearchParent = true );

    /// <summary>
    /// Callback when adding a symbol.
    /// </summary>
    /// <remarks>
    /// Default is empty. Custom processing can be performed when adding a symbol if necessary.
    /// </remarks>
    void OnWillAdd( TSymbol symbol );

    /// <summary>
    /// Add a symbol to the table
    /// </summary>
    /// <returns>true if added, false if already exists</returns>
    bool Add( TSymbol symbol, bool overwrite = false);

    /// <summary>
    /// Adds elements from the specified collection to this table
    /// </summary>
    /// <returns>true if added all, false If one or more already exist</returns>
    bool AddRange( IEnumerable<TSymbol> symbols, bool overwrite = false );

    /// <summary>
    /// Convert to list
    /// </summary>
    List<TSymbol> ToList();

    /// <summary>
    /// Dump symbols to specified writer
    /// </summary>
    void Dump( TextWriter writer ) {}
}
