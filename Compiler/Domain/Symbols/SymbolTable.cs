using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KSPCompiler.Domain.Symbols;

public abstract class SymbolTable<TSymbol> : ISymbolTable<TSymbol> where TSymbol : SymbolBase
{
    ///
    /// <inheritdoc />
    ///
    public ISymbolTable<TSymbol>? Parent { get; set; }

    ///
    /// <inheritdoc />
    ///
    public int Count
        => table.Count;

    /// <summary>
    /// Unique index value assigned to the symbol
    /// </summary>
    protected readonly UniqueSymbolIndexGenerator uniqueIndexGenerator;

    ///
    ///  <inheritdoc />
    ///
    public IReadOnlyDictionary<SymbolName, TSymbol> Table
        => table;

    /// <summary>
    /// Table body
    /// </summary>
    protected readonly Dictionary<SymbolName, TSymbol> table = new( 512 );

    #region ctor
    // ReSharper disable MemberCanBePrivate.Global
    protected SymbolTable() : this( null, UniqueSymbolIndex.Zero ) {}

    protected SymbolTable( ISymbolTable<TSymbol>? parent ) : this( parent, UniqueSymbolIndex.Zero ) {}

    protected SymbolTable( ISymbolTable<TSymbol>? parent, UniqueSymbolIndex startUniqueIndex )
    {
        Parent               = parent;
        uniqueIndexGenerator = new UniqueSymbolIndexGenerator( startUniqueIndex );
    }
    // ReSharper restore MemberCanBePrivate.Global
    #endregion

    #region Search
    /// <summary>
    /// Searches whether the specified symbol name is registered in the table
    /// </summary>
    /// <returns>Valid instance, null if not found</returns>
    public virtual bool TrySearchByName( SymbolName name, out TSymbol result, bool enableSearchParent = true )
    {
        if( table.TryGetValue( name, out result ) )
        {
            return true;
        }

        if( !enableSearchParent )
        {
            return false;
        }

        var p = Parent;

        while( p != null )
        {
            if( p.TrySearchByName( name, out result ) )
            {
                return true;
            }

            p = p.Parent;
        }

        return false;
    }

    public virtual bool TrySearchByIndex( UniqueSymbolIndex index, out TSymbol result, bool enableSearchParent = true )
    {
        // table.Values.Any( x => x?.TableIndex == index );

        result = default!;

        // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
        foreach( var x in table.Values )
        {
            // ReSharper disable once InvertIf
            if( x?.TableIndex == index )
            {
                result = x;
                return true;
            }
        }

        if( !enableSearchParent )
        {
            return false;
        }


        var p = Parent;

        while( p != null )
        {
            if( p.TrySearchByIndex( index, out result ) )
            {
                return true;
            }

            p = p.Parent;
        }

        return false;
    }

    public virtual bool TrySearchIndexByName( SymbolName name, out UniqueSymbolIndex result, bool enableSearchParent = true )
    {
        if( table.TryGetValue( name, out var symbol ) )
        {
            result = symbol.TableIndex;
            return true;
        }

        if( !enableSearchParent )
        {
            result = UniqueSymbolIndex.Zero;
            return false;
        }

        var p = Parent;

        while( p != null )
        {
            if( p.TrySearchIndexByName( name, out result ) )
            {
                return true;
            }

            p = p.Parent;
        }

        result = UniqueSymbolIndex.Zero;
        return false;
    }
    #endregion ~Search

    #region Adding

    ///
    /// <inheritdoc />
    ///
    public bool Add( TSymbol symbol, bool overwrite = false )
    {
        var contains = table.ContainsKey( symbol.Name );

        if( contains && !overwrite)
        {
            // Already added
            return false;
        }

        OnWillAdd( symbol );
        symbol.TableIndex    = uniqueIndexGenerator.Next();
        table[ symbol.Name ] = symbol;
        return true;
    }

    ///
    /// <inheritdoc />
    ///
    public bool AddRange( IEnumerable<TSymbol> symbols, bool overwrite = false )
    {
        var result = true;

        foreach( var x in symbols )
        {
            result &= Add( x, overwrite );
        }

        return result;
    }
    #endregion ~Adding

    #region Removing
    public bool Remove( TSymbol symbol )
    {
        if( !table.ContainsKey( symbol.Name ) )
        {
            return false;
        }

        OnWillRemove( symbol );
        return table.Remove( symbol.Name );
    }

    public void Clear()
    {
        OnWillClear();
        table.Clear();
    }
    #endregion

    #region Convert
    ///
    /// <inheritdoc />
    ///
    public virtual List<TSymbol> ToList()
        => table.Values.ToList();
    #endregion ~Convert

    #region IEnumerable<T>
    public virtual IEnumerator<TSymbol> GetEnumerator()
        => table.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
    #endregion

    #region Callback

    /// <summary>
    /// Callback when adding a symbol.
    /// </summary>
    /// <remarks>
    /// Default is empty. Custom processing can be performed when adding a symbol if necessary.
    /// </remarks>
    // ReSharper disable once VirtualMemberNeverOverridden.Global
    protected virtual void OnWillAdd( TSymbol symbol )
    {
        // Do nothing
    }

    /// <summary>
    /// Callback when removing a symbol.
    /// </summary>
    /// <remarks>
    /// Default is empty. Custom processing can be performed when removing a symbol if necessary.
    /// </remarks>
    // ReSharper disable once VirtualMemberNeverOverridden.Global
    protected virtual void OnWillRemove( TSymbol symbol )
    {
        // Do nothing
    }

    /// <summary>
    /// Callback when all symbols are removing.
    /// </summary>
    /// <remarks>
    /// Default is empty. Custom processing can be performed when removing all symbols if necessary.
    /// </remarks>
    // ReSharper disable once VirtualMemberNeverOverridden.Global
    protected virtual void OnWillClear()
    {
        // Do nothing
    }

    #endregion

    #region Debugging
    ///
    /// <inheritdoc />
    ///
    public virtual void Dump(TextWriter writer)
    {
        foreach( var x in table.Values )
        {
            writer.WriteLine( x );
        }
    }
    #endregion ~Debugging
}
