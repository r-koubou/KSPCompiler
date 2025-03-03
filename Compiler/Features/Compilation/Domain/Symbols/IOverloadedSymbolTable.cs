using System;
using System.Collections.Generic;
using System.IO;

namespace KSPCompiler.Domain.Symbols;

// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedMember.Global
/// <summary>
/// This symbol table is used when the same symbol name exists in multiple overloads (e.g. callback declarations)
/// </summary>
/// <typeparam name="TSymbol"></typeparam>
/// <typeparam name="TOverload"></typeparam>
public interface IOverloadedSymbolTable<TSymbol, TOverload>
    : IEnumerable<IReadOnlyDictionary<TOverload, TSymbol>>
    where TSymbol : SymbolBase
    where TOverload : IEquatable<TOverload>
{
    /// <summary>
    /// Symbol table (read-only)
    /// </summary>
    IReadOnlyDictionary<SymbolName, IReadOnlyDictionary<TOverload, TSymbol>> Table { get; }

    /// <summary>
    /// Size of symbol table
    /// </summary>
    int Count { get; }

    /// <summary>
    /// Parent node to be used when local scope, such as nesting, is allowed.
    /// </summary>
    IOverloadedSymbolTable<TSymbol, TOverload>? Parent { get; set; }

    /// <summary>
    /// Get the no overload value
    /// </summary>
    /// <remarks>
    /// If symbol has no overload, this value is used instead.
    /// </remarks>
    /// <seealso cref="AddAsNoOverload"/>
    TOverload NoOverloadValue { get; }

    /// <summary>
    /// Searches whether the specified symbol name
    /// </summary>
    /// <remarks>
    /// This method attempts to search for symbols that are expected to have no overloads.
    /// </remarks>
    /// <returns>Valid instance, null if not found</returns>
    bool TryGet( SymbolName name, out TSymbol result, bool enableSearchParent = true );

    /// <summary>
    /// Searches whether the specified symbol name and overload is registered in the table
    /// </summary>
    /// <returns>Valid instance, null if not found</returns>
    bool TryGet( SymbolName name, TOverload overload, out TSymbol result, bool enableSearchParent = true );

    /// <summary>
    /// Searches whether the specified symbol name is registered in the table
    /// </summary>
    /// <returns>Valid instance, null if not found</returns>
    bool TrySearchByName( SymbolName name, out IReadOnlyCollection<TSymbol> result, bool enableSearchParent = true );

    /// <summary>
    /// Searches whether the specified symbol index is registered in the table
    /// </summary>
    bool TrySearchByIndex( UniqueSymbolIndex index, out TSymbol result, bool enableSearchParent = true );

    /// <summary>
    /// Searches whether the specified symbol name and index is registered in the table
    /// </summary>
    bool TrySearchIndexByName( SymbolName name, out IReadOnlyCollection<UniqueSymbolIndex> result, bool enableSearchParent = true );

    /// <summary>
    /// Searches whether the specified symbol name and overload is registered in the table
    /// </summary>
    bool TryGetNoOverloadIndexByName( SymbolName name, out UniqueSymbolIndex result, bool enableSearchParent = true );

    /// <summary>
    /// Searches whether the specified symbol name and overload is registered in the table
    /// </summary>
    bool TryGetOverloadIndexByName( SymbolName name, TOverload overload, out UniqueSymbolIndex result, bool enableSearchParent = true );

    /// <summary>
    /// Add a symbol to the table without overload
    /// </summary>
    /// <remarks>
    /// NOTE: If adding symbol by this method, the overload value is set with <see cref="NoOverloadValue"/> and cannot add overload later.
    /// </remarks>
    /// <returns>true if added, false if already exists</returns>
    bool AddAsNoOverload( TSymbol symbol, bool overwrite = false);

    /// <summary>
    /// Add a symbol to the table
    /// </summary>
    /// <returns>true if added, false if already exists or added as no overload</returns>
    bool AddAsOverload( TSymbol symbol, TOverload overload, bool overwrite = false);

    // /// <summary>
    // /// Adds elements from the specified collection to this table
    // /// </summary>
    // /// <returns>Symbols for which the addition failed</returns>
    // IReadOnlyList<TSymbol> AddRangeAsNoOverload( IEnumerable<TSymbol> symbols, bool overwrite = false );
    //
    // /// <summary>
    // /// Adds elements from the specified collection to this table
    // /// </summary>
    // /// <returns>Symbols for which the addition failed</returns>
    // IReadOnlyList<TSymbol> AddRangeAsOverload( (IEnumerable<TSymbol> symbols, IEnumerable<TOverload> overloads) items, bool overwrite = false );

    /// <summary>
    /// Remove a symbol to the table
    /// </summary>
    /// <returns>true if added, false if already exists</returns>
    bool Remove( TSymbol symbol );

    /// <summary>
    /// Remove a symbol to the table
    /// </summary>
    /// <returns>true if added, false if already exists</returns>
    bool Remove( TSymbol symbol, TOverload overload );

    /// <summary>
    /// Remove all symbols from the table
    /// </summary>
    void Clear();

    /// <summary>
    /// Convert to list
    /// </summary>
    List<TSymbol> ToList();

    /// <summary>
    /// Check one or more overloads are contained in the symbol table by specified symbol name.
    /// </summary>
    /// <param name="name">A symbol name</param>
    /// <returns>True if contains at least one overload in the symbol table</returns>
    bool Contains( SymbolName name );

    /// <summary>
    /// Check contains the specified symbol by name and overload.
    /// </summary>
    /// <param name="name">A symbol name</param>
    /// <param name="overload">A overload value</param>
    /// <returns>True if contains given overload in the symbol table</returns>
    bool Contains( SymbolName name, TOverload overload );

    /// <summary>
    /// Dump symbols to specified writer
    /// </summary>
    void Dump( TextWriter writer ) {}
}
