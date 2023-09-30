using System;
using System.Collections.Generic;
using System.IO;

namespace KSPCompiler.Domain.Symbols;

public interface ISymbolTable<TSymbol> : ICloneable where TSymbol : SymbolBase
{
    /// <summary>
    /// Parent node to be used when local scope, such as nesting, is allowed.
    /// </summary>
    SymbolTable<TSymbol>? Parent { get; set; }

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
    bool Add( SymbolName name, TSymbol symbol );

    /// <summary>
    /// Convert to list
    /// </summary>
    List<TSymbol> ToList();

    /// <summary>
    /// Dump symbols to specified writer
    /// </summary>
    void Dump( TextWriter writer ) {}
}
