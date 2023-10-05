using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KSPCompiler.Domain.Symbols;

public abstract class SymbolTable<TSymbol> : ISymbolTable<TSymbol> where TSymbol : SymbolBase
{
    /// <summary>
    /// Parent node to be used when local scope, such as nesting, is allowed.
    /// </summary>
    public SymbolTable<TSymbol>? Parent { get; set; }

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

    protected SymbolTable( SymbolTable<TSymbol>? parent ) : this( parent, UniqueSymbolIndex.Zero ) {}

    protected SymbolTable( SymbolTable<TSymbol>? parent, UniqueSymbolIndex startUniqueIndex )
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
    public abstract bool Add( TSymbol symbol );

    ///
    /// <inheritdoc />
    ///
    public virtual void Merge( ISymbolTable<TSymbol> other )
    {
        foreach( var key in other.Table.Keys )
        {
            table.TryAdd( key, other.Table[ key ] );
        }
    }
    #endregion ~Adding

    #region Convert
    ///
    /// <inheritdoc />
    ///
    public virtual List<TSymbol> ToList()
        => table.Values.ToList();
    #endregion ~Convert

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
