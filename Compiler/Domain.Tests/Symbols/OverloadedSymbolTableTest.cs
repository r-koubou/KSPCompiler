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
    private CallbackSymbolTable symbolTable;

    [SetUp]
    public void SetUp()
    {
        symbolTable = new CallbackSymbolTable();
    }

    #region Get by no overload
    [Test]
    public void TableCanSearchWithNoOverload()
    {
        var symbol = MockUtility.CreateCallbackSymbol( "init" );
        symbolTable.AddAsNoOverload( symbol );

        var result = symbolTable.TryGet( new SymbolName( "init" ), out _ );
        Assert.That( result, Is.True );
    }

    [Test]
    public void TableCannotSearchNoExistWithNoOverload()
    {
        var symbol = MockUtility.CreateCallbackSymbol( "init" );
        var overload1 = MockUtility.CreateArgumentSymbol( "overload1" );
        var overload2 = MockUtility.CreateArgumentSymbol( "overload2" );

        symbolTable.AddAsOverload( symbol, overload1.Name );
        symbolTable.AddAsOverload( symbol, overload2.Name );

        var result = symbolTable.TryGet( new SymbolName( "init" ), new SymbolName( "xyz" ), out _ );
        Assert.That( result, Is.False );
    }
    #endregion ~Find with no overload

    #region Get by overload
    [Test]
    public void TableCannotSearchNoExistWithOverload()
    {
        const string callbackName = "init";

        Assert.That( symbolTable.TryGet( new SymbolName( callbackName ), out _ ), Is.False );
    }
    #endregion ~Find

    #region Search by Name
    [Test]
    public void SymbolCanSearchByName()
    {
        const string callbackName1 = "init";
        var callback1 = MockUtility.CreateCallbackSymbol( callbackName1 );

        const string callbackName2 = "ui_control";
        const string overloadName = "$overload1";
        var callback2 = MockUtility.CreateCallbackSymbol( callbackName2 );
        callback2.Arguments.Add( MockUtility.CreateArgumentSymbol( overloadName ) );

        symbolTable.AddAsNoOverload( callback1 );
        symbolTable.AddAsOverload( callback2, overloadName );

        const string searchCallbackName = callbackName1;
        Assert.That( symbolTable.TrySearchByName( searchCallbackName, out var result1 ), Is.True );
        Assert.That( result1.Count,                                                      Is.EqualTo( 1 ) );

        const string searchCallbackName2 = callbackName2;
        Assert.That( symbolTable.TrySearchByName( searchCallbackName2, out var result2 ), Is.True );
        Assert.That( result2.Count,                                                       Is.EqualTo( 1 ) );
    }

    [Test]
    public void SymbolCannotSearchByNameWithoutNoExist()
    {
        const string callbackName1 = "init";
        var callback1 = MockUtility.CreateCallbackSymbol( callbackName1 );

        const string callbackName2 = "ui_control";
        const string overloadName = "$overload1";
        var callback2 = MockUtility.CreateCallbackSymbol( callbackName2 );
        callback2.Arguments.Add( MockUtility.CreateArgumentSymbol( overloadName ) );

        symbolTable.AddAsNoOverload( callback1 );
        symbolTable.AddAsOverload( callback2, overloadName );

        const string searchCallbackName = "non_exist_name1";
        Assert.That( symbolTable.TrySearchByName( searchCallbackName, out _ ), Is.False );

        const string searchCallbackName2 = "non_exist_name2";
        Assert.That( symbolTable.TrySearchByName( searchCallbackName2, out _ ), Is.False );
    }

    #endregion

    #region Search by Index
    [Test]
    public void SymbolCanSearchByIndex()
    {
        const string callbackName1 = "init";
        var callback1 = MockUtility.CreateCallbackSymbol( callbackName1 );

        const string callbackName2 = "ui_control";
        const string overloadName = "$overload1";
        var callback2 = MockUtility.CreateCallbackSymbol( callbackName2 );
        callback2.Arguments.Add( MockUtility.CreateArgumentSymbol( overloadName ) );

        symbolTable.AddAsNoOverload( callback1 );
        symbolTable.AddAsOverload( callback2, overloadName );

        var searchIndex1 = new UniqueSymbolIndex( 0 );
        Assert.That( symbolTable.TrySearchByIndex( searchIndex1, out _ ), Is.True );

        var searchIndex2 = new UniqueSymbolIndex( 1 );
        Assert.That( symbolTable.TrySearchByIndex( searchIndex2, out _ ), Is.True );
    }

    [Test]
    public void SymbolCannotSearchByIndexWithoutNoExist()
    {
        const string callbackName1 = "init";
        var callback1 = MockUtility.CreateCallbackSymbol( callbackName1 );

        const string callbackName2 = "ui_control";
        const string overloadName = "$overload1";
        var callback2 = MockUtility.CreateCallbackSymbol( callbackName2 );
        callback2.Arguments.Add( MockUtility.CreateArgumentSymbol( overloadName ) );

        symbolTable.AddAsNoOverload( callback1 );
        symbolTable.AddAsOverload( callback2, overloadName );

        var searchIndex1 = new UniqueSymbolIndex( 123 );
        Assert.That( symbolTable.TrySearchByIndex( searchIndex1, out _ ), Is.False );

        var searchIndex2 = new UniqueSymbolIndex( 456 );
        Assert.That( symbolTable.TrySearchByIndex( searchIndex2, out _ ), Is.False );
    }

    #endregion ~Search by Index

    #region Search indexes by name
    [Test]
    public void SymbolCanSearchIndexesByName()
    {
        const string callbackName1 = "init";
        var callback1 = MockUtility.CreateCallbackSymbol( callbackName1 );

        const string callbackName2 = "ui_control";
        const string overloadName = "$overload1";
        var callback2 = MockUtility.CreateCallbackSymbol( callbackName2 );
        callback2.Arguments.Add( MockUtility.CreateArgumentSymbol( overloadName ) );

        symbolTable.AddAsNoOverload( callback1 );
        symbolTable.AddAsOverload( callback2, overloadName );

        var expextedIndex1 = new UniqueSymbolIndex( 0 );
        Assert.That( symbolTable.TrySearchIndexByName( callbackName1, out var result1 ), Is.True );
        Assert.That( result1.Contains( expextedIndex1 ),                                 Is.True );

        var expextedIndex2 = new UniqueSymbolIndex( 1 );
        Assert.That( symbolTable.TrySearchIndexByName( callbackName2, out var result2 ), Is.True );
        Assert.That( result2.Contains( expextedIndex2 ),                                 Is.True );
    }

    [Test]
    public void SymbolCannotSearchIndexesByNameWithoutNoExist()
    {
        const string callbackName1 = "init";
        var callback = MockUtility.CreateCallbackSymbol( callbackName1 );

        symbolTable.AddAsNoOverload( callback );

        const string callbackName2 = "ui_control";
        const string overloadName = "$overload1";
        var callback2 = MockUtility.CreateCallbackSymbol( callbackName2 );

        callback2.Arguments.Add( MockUtility.CreateArgumentSymbol( overloadName ) );
        symbolTable.AddAsOverload( callback2, overloadName );

        const string searchCallbackName1 = "non_exist_name";
        Assert.That( symbolTable.TrySearchIndexByName( searchCallbackName1, out _ ), Is.False );
    }
    #endregion ~Search index by name

    #region Get index by name
    [Test]
    public void SymbolCanGetIndexByName()
    {
        const string callbackName1 = "init";
        var callback1 = MockUtility.CreateCallbackSymbol( callbackName1 );

        const string callbackName2 = "ui_control";
        const string overloadName = "$overload1";
        var callback2 = MockUtility.CreateCallbackSymbol( callbackName2 );
        callback2.Arguments.Add( MockUtility.CreateArgumentSymbol( overloadName ) );

        symbolTable.AddAsNoOverload( callback1 );
        symbolTable.AddAsOverload( callback2, overloadName );

        var expectedIndex1 = new UniqueSymbolIndex( 0 );
        Assert.That( symbolTable.TryGetNoOverloadIndexByName( callbackName1, out var result1 ), Is.True );
        Assert.That( result1,                                                                   Is.EqualTo( expectedIndex1 ) );

        var expectedIndex2 = new UniqueSymbolIndex( 1 );
        Assert.That( symbolTable.TryGetOverloadIndexByName( callbackName2, overloadName, out var result2 ), Is.True );
        Assert.That( result2,                                                                               Is.EqualTo( expectedIndex2 ) );
    }

    [Test]
    public void SymbolCannotGetIndexByNameWithoutNoExist()
    {
        const string callbackName1 = "init";
        var callback1 = MockUtility.CreateCallbackSymbol( callbackName1 );

        const string callbackName2 = "ui_control";
        const string overloadName = "$overload1";
        var callback2 = MockUtility.CreateCallbackSymbol( callbackName2 );
        callback2.Arguments.Add( MockUtility.CreateArgumentSymbol( overloadName ) );

        symbolTable.AddAsNoOverload( callback1 );
        symbolTable.AddAsOverload( callback2, overloadName );

        const string searchCallbackName1 = "non_exist_name1";
        Assert.That( symbolTable.TryGetNoOverloadIndexByName( searchCallbackName1, out _ ), Is.False );

        const string searchCallbackName2 = "non_exist_name2";
        Assert.That( symbolTable.TryGetOverloadIndexByName( searchCallbackName2, overloadName, out _ ), Is.False );
    }
    #endregion ~Get index by name

    #region Add No Overload and Overload mixed
    [Test]
    public void SymbolCanAdd()
    {
        const string callbackName1 = "init";
        var callback1 = MockUtility.CreateCallbackSymbol( callbackName1 );

        const string callbackName2 = "ui_control";
        const string overloadName = "$overload1";
        var callback2 = MockUtility.CreateCallbackSymbol( callbackName2, "$arg" );

        Assert.That( symbolTable.AddAsNoOverload( callback1 ), Is.True );
        Assert.That( symbolTable.AddAsOverload( callback2, overloadName ), Is.True );
        Assert.That( symbolTable.Contains( callbackName1 ) );
        Assert.That( symbolTable.Contains( callbackName2, overloadName ) );
    }
    #endregion ~Add No Overload and Overload mixed

    #region Add No Overload
    [Test]
    public void SymbolCanAddWithoutOverload()
    {
        const string callbackName = "init";

        var callback = MockUtility.CreateCallbackSymbol( callbackName );
        var result = symbolTable.AddAsNoOverload( callback );

        Assert.That( result,                                Is.True );
        Assert.That( symbolTable.Contains( callback.Name ), Is.True );
    }

    [Test]
    public void SymbolCannotAddOverloadWhenAddAsNoOverloadOnce()
    {
        const string callbackName = "ui_control";
        const string overloadName = "$overload1";

        var callback = MockUtility.CreateCallbackSymbol( callbackName, overloadName );

        Assert.That( symbolTable.AddAsNoOverload( callback ),                        Is.True );
        Assert.That( symbolTable.AddAsNoOverload( callback ),                        Is.False );
        Assert.That( symbolTable.AddAsOverload( callback, new SymbolName( "abc" ) ), Is.False );
    }
    #endregion ~Add No Overload

    #region Add Overload
    [Test]
    public void SymbolCanAddWithOverload()
    {
        const string callbackName = "ui_control";
        const string overloadName1 = "$overload1";
        const string overloadName2 = "$overload2";

        var callback1 = MockUtility.CreateCallbackSymbol( callbackName, overloadName1 );
        var callback2 = MockUtility.CreateCallbackSymbol( callbackName, overloadName2 );

        Assert.That( symbolTable.AddAsOverload( callback1, overloadName1 ), Is.True );
        Assert.That( symbolTable.AddAsOverload( callback2, overloadName2 ), Is.True );

        Assert.That( symbolTable.Contains( callbackName, overloadName1 ), Is.True );
        Assert.That( symbolTable.Contains( callbackName, overloadName2 ), Is.True );
    }

    [Test]
    public void SymbolCannotDuplicateAddWithOverload()
    {
        const string callbackName = "ui_control";
        const string overloadName = "$overload1";

        var callback = MockUtility.CreateCallbackSymbol( callbackName );
        Assert.That( symbolTable.AddAsOverload( callback, overloadName ), Is.True );
        Assert.That( symbolTable.AddAsOverload( callback, overloadName ), Is.False, "Duplicate add should return false" );
    }
    #endregion ~Add Overload

    #region Remove
    [Test]
    public void SymbolCanRemove()
    {
        const string callbackName1 = "init";
        var callback1 = MockUtility.CreateCallbackSymbol( callbackName1 );

        const string callbackName2 = "ui_control";
        const string overloadName1 = "$overload1";
        const string overloadName2 = "$overload2";

        var callback2 = MockUtility.CreateCallbackSymbol( callbackName2 );
        callback2.Arguments.Add( MockUtility.CreateArgumentSymbol( overloadName1 ) );

        symbolTable.AddAsNoOverload( callback1 );
        symbolTable.AddAsOverload( callback2, overloadName1 );
        symbolTable.AddAsOverload( callback2, overloadName2 );

        Assert.That( symbolTable.Remove( callback1 ), Is.True );
        Assert.That( symbolTable.Count,               Is.EqualTo( 2 ) );

        Assert.That( symbolTable.Remove( callback2, overloadName1 ), Is.True );
        Assert.That( symbolTable.Count,                              Is.EqualTo( 1 ) );

        Assert.That( symbolTable.Contains( callbackName1 ),                Is.False );
        Assert.That( symbolTable.Contains( callbackName2, overloadName1 ), Is.False );
    }

    [Test]
    public void SymbolCannotRemoveNoExist()
    {
        const string callbackName1 = "init";
        var callback1 = MockUtility.CreateCallbackSymbol( callbackName1 );

        const string callbackName2 = "ui_control";
        const string overloadName = "$overload1";
        var callback2 = MockUtility.CreateCallbackSymbol( callbackName2 );
        callback2.Arguments.Add( MockUtility.CreateArgumentSymbol( overloadName ) );

        symbolTable.AddAsNoOverload( callback1 );
        symbolTable.AddAsOverload( callback2, overloadName );

        var nonExistSymbol = MockUtility.CreateCallbackSymbol( "non_exist" );

        Assert.That( symbolTable.Remove( nonExistSymbol ),               Is.False );
        Assert.That( symbolTable.Remove( nonExistSymbol, overloadName ), Is.False );

        Assert.That( symbolTable.Count, Is.EqualTo( 2 ) );

    }

    [Test]
    public void AllSymbolsCanClear()
    {
        const string callbackName1 = "init";
        var callback1 = MockUtility.CreateCallbackSymbol( callbackName1 );

        const string callbackName2 = "ui_control";
        const string overloadName = "$overload1";
        var callback2 = MockUtility.CreateCallbackSymbol( callbackName2 );
        callback2.Arguments.Add( MockUtility.CreateArgumentSymbol( overloadName ) );

        symbolTable.AddAsNoOverload( callback1 );
        symbolTable.AddAsOverload( callback2, overloadName );

        symbolTable.Clear();

        Assert.That( symbolTable.Count, Is.EqualTo( 0 ) );
    }
    #endregion ~Remove

    #region Convert
    [Test]
    public void SymbolTableToListConvertTest()
    {
        const string callbackName1 = "init";
        var callback1 = MockUtility.CreateCallbackSymbol( callbackName1 );

        const string callbackName2 = "ui_control";
        const string overloadName = "$overload1";
        var callback2 = MockUtility.CreateCallbackSymbol( callbackName2 );
        callback2.Arguments.Add( MockUtility.CreateArgumentSymbol( overloadName ) );

        symbolTable.AddAsNoOverload( callback1 );
        symbolTable.AddAsOverload( callback2, overloadName );

        var list = symbolTable.ToList();

        Assert.That( list.Count, Is.EqualTo( 2 ) );

        Assert.That( list[ 0 ].Name.Value, Is.EqualTo( callbackName1 ) );
        Assert.That( list[ 1 ].Name.Value, Is.EqualTo( callbackName2 ) );
    }
    #endregion ~Convert
}

public class CallbackSymbolTable : OverloadedSymbolTable<CallbackSymbol, SymbolName>
{
    public CallbackSymbolTable(
        IOverloadedSymbolTable<CallbackSymbol, SymbolName>? parent = null
    ) : base( parent ) {}

    public CallbackSymbolTable(
        UniqueSymbolIndex startUniqueIndex,
        IOverloadedSymbolTable<CallbackSymbol, SymbolName>? parent = null
    ) : base( startUniqueIndex, parent ) {}

    public override SymbolName NoOverloadValue
        => SymbolName.Empty;
}

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
    public bool TryGet( SymbolName name, out TSymbol result, bool enableSearchParent = true )
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
