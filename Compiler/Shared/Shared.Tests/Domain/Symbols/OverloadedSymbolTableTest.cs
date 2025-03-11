using System.Linq;

using KSPCompiler.Shared.Domain.Compilation.Symbols;

using NUnit.Framework;

namespace KSPCompiler.Shared.Tests.Domain.Symbols;

[TestFixture]
public class CallbackSymbolTableTest
{
    private CallbackSymbolTable symbolTable = null!;

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
        var overload1 = MockUtility.CreateArgumentSymbolList( "overload1" );
        var overload2 = MockUtility.CreateArgumentSymbolList( "overload2" );


        symbolTable.AddAsOverload( symbol, overload1 );
        symbolTable.AddAsOverload( symbol, overload2 );

        var notExisiList = MockUtility.CreateArgumentSymbolList( "not_exist" );

        var result = symbolTable.TryGet( new SymbolName( "init" ), notExisiList, out _ );
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
        var overload = MockUtility.CreateArgumentSymbolList( "overload1" );

        var callback2 = MockUtility.CreateCallbackSymbol( callbackName2 );
        callback2.Arguments.Add( overload[ 0 ] );

        symbolTable.AddAsNoOverload( callback1 );
        symbolTable.AddAsOverload( callback2, overload );

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
        var overload = MockUtility.CreateArgumentSymbolList( "overload1" );
        var callback2 = MockUtility.CreateCallbackSymbol( callbackName2 );
        callback2.Arguments.Add( overload[ 0 ] );

        symbolTable.AddAsNoOverload( callback1 );
        symbolTable.AddAsOverload( callback2, overload );

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
        var overload = MockUtility.CreateArgumentSymbolList( "overload1" );
        var callback2 = MockUtility.CreateCallbackSymbol( callbackName2 );
        callback2.Arguments.Add( overload[ 0 ] );

        symbolTable.AddAsNoOverload( callback1 );
        symbolTable.AddAsOverload( callback2, overload );

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
        var overload = MockUtility.CreateArgumentSymbolList( "overload1" );
        var callback2 = MockUtility.CreateCallbackSymbol( callbackName2 );
        callback2.Arguments.Add( overload[ 0 ] );

        symbolTable.AddAsNoOverload( callback1 );
        symbolTable.AddAsOverload( callback2, overload );

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
        var overload = MockUtility.CreateArgumentSymbolList( "overload1" );
        var callback2 = MockUtility.CreateCallbackSymbol( callbackName2 );
        callback2.Arguments.Add( overload[ 0 ] );

        symbolTable.AddAsNoOverload( callback1 );
        symbolTable.AddAsOverload( callback2, overload );

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

        var overload = MockUtility.CreateArgumentSymbolList( overloadName );
        var callback2 = MockUtility.CreateCallbackSymbol( callbackName2 );

        callback2.Arguments.Add( MockUtility.CreateArgumentSymbol( overloadName ) );
        symbolTable.AddAsOverload( callback2, overload );

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
        var overload = MockUtility.CreateArgumentSymbolList( "overload1" );
        var callback2 = MockUtility.CreateCallbackSymbol( callbackName2 );
        callback2.Arguments.Add( overload[ 0 ] );

        symbolTable.AddAsNoOverload( callback1 );
        symbolTable.AddAsOverload( callback2, overload );

        var expectedIndex1 = new UniqueSymbolIndex( 0 );
        Assert.That( symbolTable.TryGetNoOverloadIndexByName( callbackName1, out var result1 ), Is.True );
        Assert.That( result1,                                                                   Is.EqualTo( expectedIndex1 ) );

        var expectedIndex2 = new UniqueSymbolIndex( 1 );
        Assert.That( symbolTable.TryGetOverloadIndexByName( callbackName2, overload, out var result2 ), Is.True );
        Assert.That( result2,                                                                           Is.EqualTo( expectedIndex2 ) );
    }

    [Test]
    public void SymbolCannotGetIndexByNameWithoutNoExist()
    {
        const string callbackName1 = "init";
        var callback1 = MockUtility.CreateCallbackSymbol( callbackName1 );

        const string callbackName2 = "ui_control";
        var overload = MockUtility.CreateArgumentSymbolList( "overload1" );
        var callback2 = MockUtility.CreateCallbackSymbol( callbackName2 );
        callback2.Arguments.Add( overload[ 0 ] );

        symbolTable.AddAsNoOverload( callback1 );
        symbolTable.AddAsOverload( callback2, overload );

        const string searchCallbackName1 = "non_exist_name1";
        Assert.That( symbolTable.TryGetNoOverloadIndexByName( searchCallbackName1, out _ ), Is.False );

        const string searchCallbackName2 = "non_exist_name2";
        Assert.That( symbolTable.TryGetOverloadIndexByName( searchCallbackName2, overload, out _ ), Is.False );
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

        var overload = MockUtility.CreateArgumentSymbolList( overloadName );
        var callback2 = MockUtility.CreateCallbackSymbol( callbackName2, "$arg" );
        callback2.Arguments.Add( overload[ 0 ] );

        Assert.That( symbolTable.AddAsNoOverload( callback1 ),         Is.True );
        Assert.That( symbolTable.AddAsOverload( callback2, overload ), Is.True );
        Assert.That( symbolTable.Contains( callbackName1 ) );
        Assert.That( symbolTable.Contains( callbackName2, overload ) );
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

        Assert.That( symbolTable.AddAsNoOverload( callback ),   Is.True );
        Assert.That( symbolTable.AddAsNoOverload( callback ),   Is.False );
        Assert.That( symbolTable.AddAsOverload( callback, [] ), Is.False );
    }
    #endregion ~Add No Overload

    #region Add Overload
    [Test]
    public void SymbolCanAddWithOverload()
    {
        const string callbackName = "ui_control";
        const string overloadName1 = "$overload1";
        const string overloadName2 = "$overload2";

        var overload1 = MockUtility.CreateArgumentSymbolList( overloadName1 );
        var overload2 = MockUtility.CreateArgumentSymbolList( overloadName2 );

        var callback1 = MockUtility.CreateCallbackSymbol( callbackName, overloadName1 );
        var callback2 = MockUtility.CreateCallbackSymbol( callbackName, overloadName2 );

        Assert.That( symbolTable.AddAsOverload( callback1, overload1 ), Is.True );
        Assert.That( symbolTable.AddAsOverload( callback2, overload2 ), Is.True );

        Assert.That( symbolTable.Contains( callbackName, overload1 ), Is.True );
        Assert.That( symbolTable.Contains( callbackName, overload2 ), Is.True );
    }

    [Test]
    public void SymbolCannotDuplicateAddWithOverload()
    {
        const string callbackName = "ui_control";
        const string overloadName = "$overload1";

        var overload = MockUtility.CreateArgumentSymbolList( overloadName );
        var callback = MockUtility.CreateCallbackSymbol( callbackName );

        Assert.That( symbolTable.AddAsOverload( callback, overload ), Is.True );
        Assert.That( symbolTable.AddAsOverload( callback, overload ), Is.False, "Duplicate add should return false" );
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

        var overload1 = MockUtility.CreateArgumentSymbolList( overloadName1 );
        var overload2 = MockUtility.CreateArgumentSymbolList( overloadName2 );

        var callback2 = MockUtility.CreateCallbackSymbol( callbackName2 );
        callback2.Arguments.Add( MockUtility.CreateArgumentSymbol( overloadName1 ) );

        symbolTable.AddAsNoOverload( callback1 );
        symbolTable.AddAsOverload( callback2, overload1 );
        symbolTable.AddAsOverload( callback2, overload2 );

        Assert.That( symbolTable.Remove( callback1 ), Is.True );
        Assert.That( symbolTable.Count,               Is.EqualTo( 2 ) );

        Assert.That( symbolTable.Remove( callback2, overload1 ), Is.True );
        Assert.That( symbolTable.Count,                          Is.EqualTo( 1 ) );

        Assert.That( symbolTable.Remove( callback2, overload2 ), Is.True );
        Assert.That( symbolTable.Count,                          Is.EqualTo( 0 ) );

        Assert.That( symbolTable.Contains( callbackName1 ),            Is.False );
        Assert.That( symbolTable.Contains( callbackName2, overload1 ), Is.False );
    }

    [Test]
    public void SymbolCannotRemoveNoExist()
    {
        const string callbackName1 = "init";
        var callback1 = MockUtility.CreateCallbackSymbol( callbackName1 );

        const string callbackName2 = "ui_control";
        const string overloadName = "$overload1";

        var overload = MockUtility.CreateArgumentSymbolList( overloadName );
        var callback2 = MockUtility.CreateCallbackSymbol( callbackName2 );
        callback2.Arguments.Add( MockUtility.CreateArgumentSymbol( overloadName ) );

        symbolTable.AddAsNoOverload( callback1 );
        symbolTable.AddAsOverload( callback2, overload );

        var nonExistSymbol = MockUtility.CreateCallbackSymbol( "non_exist" );

        Assert.That( symbolTable.Remove( nonExistSymbol ),           Is.False );
        Assert.That( symbolTable.Remove( nonExistSymbol, overload ), Is.False );

        Assert.That( symbolTable.Count, Is.EqualTo( 2 ) );
    }

    [Test]
    public void AllSymbolsCanClear()
    {
        const string callbackName1 = "init";
        var callback1 = MockUtility.CreateCallbackSymbol( callbackName1 );

        const string callbackName2 = "ui_control";
        const string overloadName = "$overload1";

        var overload = MockUtility.CreateArgumentSymbolList( overloadName );
        var callback2 = MockUtility.CreateCallbackSymbol( callbackName2 );
        callback2.Arguments.Add( MockUtility.CreateArgumentSymbol( overloadName ) );

        symbolTable.AddAsNoOverload( callback1 );
        symbolTable.AddAsOverload( callback2, overload );

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

        var overload = MockUtility.CreateArgumentSymbolList( overloadName );
        var callback2 = MockUtility.CreateCallbackSymbol( callbackName2 );
        callback2.Arguments.Add( MockUtility.CreateArgumentSymbol( overloadName ) );

        symbolTable.AddAsNoOverload( callback1 );
        symbolTable.AddAsOverload( callback2, overload );

        var list = symbolTable.ToList();

        Assert.That( list.Count, Is.EqualTo( 2 ) );

        Assert.That( list[ 0 ].Name.Value, Is.EqualTo( callbackName1 ) );
        Assert.That( list[ 1 ].Name.Value, Is.EqualTo( callbackName2 ) );
    }
    #endregion ~Convert
}
