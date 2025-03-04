using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace KSPCompiler.Shared.Domain.Symbols;

public abstract class OverloadedSymbolTable<TSymbol, TOverload>(
    UniqueSymbolIndex startUniqueIndex,
    IOverloadedSymbolTable<TSymbol, TOverload>? parent
) : IOverloadedSymbolTable<TSymbol, TOverload>
    where TSymbol : SymbolBase
    where TOverload : IEquatable<TOverload>
{
    // ReSharper disable once MemberCanBePrivate.Global
    protected readonly Dictionary<SymbolName, Dictionary<TOverload, TSymbol>> table = new();

    /// <summary>
    /// Unique index value assigned to the symbol
    /// </summary>
    private readonly UniqueSymbolIndexGenerator uniqueIndexGenerator = new( startUniqueIndex );

    public virtual IReadOnlyDictionary<SymbolName, IReadOnlyDictionary<TOverload, TSymbol>> Table
        => table.ToDictionary(
            x => x.Key,
            IReadOnlyDictionary<TOverload, TSymbol> ( x ) => x.Value
        );

    public virtual int Count
        => Table.Values.Sum( x => x.Count );

    public virtual IOverloadedSymbolTable<TSymbol, TOverload>? Parent { get; set; } = parent;

    public abstract TOverload NoOverloadValue { get; }

    protected OverloadedSymbolTable(
        IOverloadedSymbolTable<TSymbol, TOverload>? parent
    ) : this( UniqueSymbolIndex.Zero, parent ) {}

    #region IEnumerator
    public IEnumerator<IReadOnlyDictionary<TOverload, TSymbol>> GetEnumerator()
    {
        foreach( var pair in table )
        {
            yield return pair.Value;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
    #endregion ~IEnumerator

    #region Find
    public virtual bool TryGet( SymbolName name, out TSymbol result, bool enableSearchParent = true )
        => TryGet( name, NoOverloadValue, out result, enableSearchParent );

    public virtual bool TryGet( SymbolName name, TOverload overload, out TSymbol result, bool enableSearchParent = true )
    {
        result = null!;

        if( !table.TryGetValue( name, out var overloads ) )
        {
            return false;
        }

        if( overloads.TryGetValue( overload, out result! ) )
        {
            return true;
        }

        if( !enableSearchParent || Parent == null )
        {
            return false;
        }

        return Parent.TryGet( name, overload, out result, enableSearchParent );
    }

    public virtual bool TrySearchByName( SymbolName name, out IReadOnlyCollection<TSymbol> result, bool enableSearchParent = true )
    {
        result = null!;

        if( !table.TryGetValue( name, out var overloads ) )
        {
            return false;
        }

        result = overloads.Values.ToList();

        if( !enableSearchParent || Parent == null )
        {
            return true;
        }

        if( Parent.TrySearchByName( name, out var parentResult, enableSearchParent ) )
        {
            result = result.Concat( parentResult ).ToList();
        }

        return true;
    }

    public virtual bool TrySearchByIndex( UniqueSymbolIndex index, out TSymbol result, bool enableSearchParent = true )
    {
        result = null!;

        foreach( var overloads in table.Values )
        {
            foreach( var symbol in overloads.Values )
            {
                if( symbol.TableIndex == index )
                {
                    result = symbol;

                    return true;
                }
            }
        }

        if( !enableSearchParent || Parent == null )
        {
            return false;
        }

        return Parent.TrySearchByIndex( index, out result, enableSearchParent );
    }

    public virtual bool TrySearchIndexByName( SymbolName name, out IReadOnlyCollection<UniqueSymbolIndex> result, bool enableSearchParent = true )
    {
        result = null!;

        if( !table.TryGetValue( name, out var overloads ) )
        {
            return false;
        }

        result = overloads.Values.Select( x => x.TableIndex ).ToList();

        if( !enableSearchParent || Parent == null )
        {
            return true;
        }

        if( Parent.TrySearchIndexByName( name, out var parentResult, enableSearchParent ) )
        {
            result = result.Concat( parentResult ).ToList();
        }

        return true;
    }

    public virtual bool TryGetNoOverloadIndexByName( SymbolName name, out UniqueSymbolIndex result, bool enableSearchParent = true )
        => TryGetOverloadIndexByName( name, NoOverloadValue, out result, enableSearchParent );

    public virtual bool TryGetOverloadIndexByName( SymbolName name, TOverload overload, out UniqueSymbolIndex result, bool enableSearchParent = true )
    {
        result = null!;

        if( !table.TryGetValue( name, out var overloads ) )
        {
            return false;
        }

        if( !overloads.TryGetValue( overload, out var symbol ) )
        {
            return false;
        }

        result = symbol.TableIndex;

        return true;
    }
    #endregion ~Find

    #region Add
    public bool AddAsNoOverload( TSymbol symbol, bool overwrite = false )
    {
        return AddAsOverload( symbol, NoOverloadValue, overwrite );
    }

    public virtual bool AddAsOverload( TSymbol symbol, TOverload overload, bool overwrite = false )
    {
        // 既にシンボル名が存在する場合
        if( table.TryGetValue( symbol.Name, out var overloads ) )
        {
            // 1つ目の登録時にオーバーロードしてい無しで登録されていた場合
            // 後からオーバーロードを追加することはできない
            if( !overwrite && overloads.Keys.Any( key => key.Equals( NoOverloadValue ) ) )
            {
                return false;
            }

            // 既にオーバーロードが存在する場合 && 上書き不可の場合
            if( !overwrite && overloads.Keys.Any( key => key.Equals( overload ) ) )
            {
                return false;
            }

            // 新規オーバーロードを追加
            OnWillAdd( symbol, overload );
            symbol.TableIndex     = uniqueIndexGenerator.Next();
            overloads[ overload ] = symbol;
        }
        // シンボル名が存在しない場合
        // 新規シンボル、オーバーロードを追加
        else
        {
            OnWillAdd( symbol, overload );
            symbol.TableIndex = uniqueIndexGenerator.Next();
            table[ symbol.Name ] = new Dictionary<TOverload, TSymbol>
            {
                { overload, symbol }
            };
        }

        return true;
    }
    #endregion ~Add

    #region Remove
    public virtual bool Remove( TSymbol symbol )
    {
        if( !Contains( symbol.Name ) )
        {
            return false;
        }

        OnWillRemove( symbol, NoOverloadValue );
        table.Remove( symbol.Name );

        return true;
    }

    public virtual bool Remove( TSymbol symbol, TOverload overload )
    {
        if( !table.TryGetValue( symbol.Name, out var overloads ) )
        {
            return false;
        }

        if( !overloads.ContainsKey( overload ) )
        {
            return false;
        }

        OnWillRemove( symbol, overload );
        overloads.Remove( overload );

        if( !table.TryGetValue( symbol.Name, out overloads ) )
        {
            return true;
        }

        // オーバーロードが0になった場合、シンボル名を削除

        if( overloads.Count == 0 )
        {
            table.Remove( symbol.Name );
        }

        return true;
    }

    public virtual void Clear()
    {
        OnWillClear();
        table.Clear();
    }
    #endregion ~Remove

    #region Convert
    public virtual List<TSymbol> ToList()
        => table.Values.SelectMany( x => x.Values ).ToList();
    #endregion ~Convert

    #region Contains
    public virtual bool Contains( SymbolName name )
    {
        return table.ContainsKey( name );
    }

    public virtual bool Contains( SymbolName name, TOverload overload )
    {
        if( !table.TryGetValue( name, out var overloads ) )
        {
            return false;
        }

        return overloads.ContainsKey( overload );
    }
    #endregion ~Contains

    #region Callback
    /// <summary>
    /// Callback when adding a symbol.
    /// </summary>
    /// <remarks>
    /// Default is empty. Custom processing can be performed when adding a symbol if necessary.
    /// </remarks>
    // ReSharper disable once VirtualMemberNeverOverridden.Global
    protected virtual void OnWillAdd( TSymbol symbol, TOverload overload )
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
    protected virtual void OnWillRemove( TSymbol symbol, TOverload overload )
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
    #endregion ~Callback
}
