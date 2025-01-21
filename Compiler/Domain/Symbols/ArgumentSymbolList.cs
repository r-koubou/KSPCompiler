using System;
using System.Collections;
using System.Collections.Generic;

namespace KSPCompiler.Domain.Symbols;

public abstract class ArgumentSymbolList<TArgumentSymbol>
    : IArgumentSymbolList<TArgumentSymbol>
    where TArgumentSymbol : ArgumentSymbol
{
    private readonly List<TArgumentSymbol> arguments = [];

    protected ArgumentSymbolList() {}

    protected ArgumentSymbolList( IEnumerable<TArgumentSymbol> initialArguments )
    {
        arguments.AddRange( initialArguments );
    }

    public virtual IEnumerator<TArgumentSymbol> GetEnumerator()
        => arguments.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    public virtual void Add( TArgumentSymbol item )
    {
        if( arguments.Contains( item ))
        {
            throw new InvalidOperationException( $"Argument {item.Name} already exists" );
        }
        arguments.Add( item );
    }

    public virtual void AddRange( IEnumerable<TArgumentSymbol> items )
    {
        foreach( var item in items )
        {
            Add( item );
        }
    }

    public virtual void Clear()
        => arguments.Clear();

    public virtual bool Contains( TArgumentSymbol item )
        => arguments.Contains( item );

    public virtual void CopyTo( TArgumentSymbol[] array, int arrayIndex )
        => arguments.CopyTo( array, arrayIndex );

    public virtual bool Remove( TArgumentSymbol item )
        => arguments.Remove( item );

    public virtual int Count
        => arguments.Count;

    public virtual bool IsReadOnly
        => false;

    public virtual int IndexOf( TArgumentSymbol item )
        => arguments.IndexOf( item );

    public virtual void Insert( int index, TArgumentSymbol item )
        => arguments.Insert( index, item );

    public virtual void RemoveAt( int index )
        => arguments.RemoveAt( index );

    public virtual TArgumentSymbol this[ int index ]
    {
        get => arguments[ index ];
        set => arguments[ index ] = value;
    }
}
