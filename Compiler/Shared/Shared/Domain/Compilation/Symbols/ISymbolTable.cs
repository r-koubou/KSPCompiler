using System.Collections.Generic;
using System.IO;

namespace KSPCompiler.Shared.Domain.Compilation.Symbols;

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
    /// Add a symbol to the table
    /// </summary>
    /// <returns>true if added, false if already exists</returns>
    bool Add( TSymbol symbol, bool overwrite = false);

    /// <summary>
    /// Adds elements from the specified collection to this table
    /// </summary>
    /// <returns>Symbols for which the addition failed</returns>
    IReadOnlyList<TSymbol> AddRange( IEnumerable<TSymbol> symbols, bool overwrite = false );

    /// <summary>
    /// Remove a symbol to the table
    /// </summary>
    /// <returns>true if added, false if already exists</returns>
    bool Remove( TSymbol symbol );

    /// <summary>
    /// Remove all symbols from the table
    /// </summary>
    void Clear();

    /// <summary>
    /// Convert to list
    /// </summary>
    List<TSymbol> ToList();

    /// <summary>
    /// Check contains the specified symbol name.
    /// </summary>
    /// <param name="name">A symbol name</param>
    /// <returns>True if contains in the symbol table</returns>
    bool Contains( SymbolName name );

    /// <summary>
    /// Dump symbols to specified writer
    /// </summary>
    void Dump( TextWriter writer ) {}
}
