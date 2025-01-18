using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using KSPCompiler.Domain.Symbols;

using NUnit.Framework;

namespace KSPCompiler.Domain.Tests.Symbols;

[TestFixture]
public class CallbackSymbolTableTest
{
    private OverloadedSymbolTable<CallbackSymbol, SymbolName> symbolTable;

    [SetUp]
    public void SetUp()
    {
        symbolTable = new CallbackSymbolTable();
    }

    #region Convinience Methods
    private static CallbackSymbol CreateSymbol( string name )
        => new CallbackSymbol( true )
        {
            Name = new SymbolName( name )
        };
    #endregion ~Convinience Methods


    #region Find
    [Test]
    public void TrySearchWithOverloadReturnsFalseWhenSymbolNotFound()
    {
        var result = symbolTable.TrySearch( new SymbolName( "Test" ), new SymbolName( "abc" ), out var symbol );
        Assert.That( result, Is.False );
        Assert.That( symbol, Is.Null );
    }
    #endregion ~Find

    #region Add No Overload
    [Test]
    public void SymbolCanAddWithoutOverload()
    {
        var symbol = CreateSymbol( "Test" );
        var result = symbolTable.AddAsNoOverload( symbol );
        Assert.That( result,                              Is.True );
        Assert.That( symbolTable.Contains( symbol.Name ), Is.True );
    }

    [Test]
    public void SymbolCannotAddOverloadWhenAddAsNoOverloadOnce()
    {
        var symbol = CreateSymbol( "Test" );
        Assert.That( symbolTable.AddAsNoOverload( symbol ),                        Is.True );
        Assert.That( symbolTable.AddAsOverload( symbol, new SymbolName( "abc" ) ), Is.False );
    }
    #endregion ~Add No Overload

    #region Add Overload
    [Test]
    public void SymbolCanAddWithOverload()
    {
        var symbol = CreateSymbol( "Test" );
        var overload1 = new SymbolName( "abc" );
        var overload2 = new SymbolName( "def" );
        Assert.That( symbolTable.AddAsOverload( symbol, overload1 ), Is.True );
        Assert.That( symbolTable.AddAsOverload( symbol, overload2 ), Is.True );
        Assert.That( symbolTable.Contains( symbol.Name, overload1 ), Is.True );
        Assert.That( symbolTable.Contains( symbol.Name, overload2 ), Is.True );
    }

    [Test]
    public void SymbolCannotDuplicateAddWithOverload()
    {
        var symbol = CreateSymbol( "Test" );
        Assert.That( symbolTable.AddAsOverload( symbol, symbol.Name ), Is.True );
        Assert.That( symbolTable.AddAsOverload( symbol, symbol.Name ), Is.False, "Duplicate add should return false" );
    }
    #endregion ~Add Overload
}

public class CallbackSymbolTable : OverloadedSymbolTable<CallbackSymbol, SymbolName>
{
    public CallbackSymbolTable(
        IOverloadedSymbolTable<CallbackSymbol, SymbolName>? parent = null
    ) : base( parent ) {}

    public CallbackSymbolTable(
        IOverloadedSymbolTable<CallbackSymbol, SymbolName>? parent,
        UniqueSymbolIndex startUniqueIndex
    ) : base( parent, startUniqueIndex ) {}

    public override SymbolName NoOverloadValue
        => SymbolName.Empty;
}

public abstract class OverloadedSymbolTable<TSymbol, TOverload>(
    IOverloadedSymbolTable<TSymbol, TOverload>? parent,
    UniqueSymbolIndex startUniqueIndex
) : IOverloadedSymbolTable<TSymbol, TOverload>
    where TSymbol : SymbolBase
    where TOverload : IEquatable<TOverload>
{
    // ReSharper disable once MemberCanBePrivate.Global
    protected readonly Dictionary<SymbolName, Dictionary<TOverload, TSymbol>> table = new();

    /// <summary>
    /// Unique index value assigned to the symbol
    /// </summary>
    protected readonly UniqueSymbolIndexGenerator uniqueIndexGenerator = new( startUniqueIndex );

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
    ) : this( parent, UniqueSymbolIndex.Zero ) {}

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
    public virtual bool TrySearch( SymbolName name, TOverload overload, out TSymbol result, bool enableSearchParent = true )
    {
        result = null!;

        if( !table.TryGetValue( name, out var overloads ) )
        {
            return false;
        }

        return overloads.TryGetValue( overload, out result! );
    }

    public virtual bool TrySearchByName( SymbolName name, out IReadOnlyCollection<TSymbol> result, bool enableSearchParent = true )
        => throw new NotImplementedException();

    public virtual bool TrySearchByIndex( UniqueSymbolIndex index, out TSymbol result, bool enableSearchParent = true )
        => throw new NotImplementedException();

    public virtual bool TrySearchIndexByName( SymbolName name, out IReadOnlyCollection<UniqueSymbolIndex> result, bool enableSearchParent = true )
        => throw new NotImplementedException();

    public virtual bool TrySearchIndexByName( SymbolName name, TOverload overload, out UniqueSymbolIndex result, bool enableSearchParent = true )
        => throw new NotImplementedException();
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
            if( !overwrite && overloads.ContainsKey( NoOverloadValue ) )
            {
                return false;
            }

            // 既にオーバーロードが存在する場合 && 上書き不可の場合
            if( !overwrite && overloads.ContainsKey( overload ) )
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
