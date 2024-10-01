using System.Collections.Generic;
using System.IO;
using System.Linq;

using KSPCompiler.Commons.Path;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Domain.Symbols.Repositories;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.UITypes;

using NUnit.Framework;

namespace KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Tests;

[TestFixture]
public class UITypeRepositoryTest
{
    private static readonly string TestDataDirectory = Path.Combine( "TestData", nameof( UITypeRepositoryTest ) );

    [SetUp]
    public void Setup()
    {
        Directory.CreateDirectory( TestDataDirectory );
    }

    private static UITypeSymbol CreateDummySymbol(string name)
    {
        var uiType = new UITypeSymbol(true, new[]
        {
            new UIInitializerArgumentSymbol
            {
                Name = "$arg1",
                Description = "Argument 1",
            }
        })
        {
            Name = name,
            Description = "Dummy Type",
            DataType = DataTypeFlag.TypeInt
        };

        return uiType;
    }

    [Test]
    public void StoreTest()
    {
        using ISymbolRepository<UITypeSymbol> repository = new UITypeSymbolRepository( new FilePath( Path.Combine( TestDataDirectory, "store.json" ) ) );

        var uiType = CreateDummySymbol( "ui_button" );
        var result = repository.Store( uiType );

        Assert.IsTrue( result.Success );
        Assert.IsTrue( result.Exception == null );
    }

    [Test]
    public void StoreListTest()
    {
        using ISymbolRepository<UITypeSymbol> repository = new UITypeSymbolRepository( new FilePath( Path.Combine( TestDataDirectory, "store_with_list.json" ) ) );

        var uiTypes = new List<UITypeSymbol>()
        {
            CreateDummySymbol( "ui_button" ),
            CreateDummySymbol( "ui_label" ),
        };

        var result = repository.Store( uiTypes );

        Assert.IsTrue( result.Success );
        Assert.IsTrue( result.Exception == null );
    }

    [Test]
    public void DeleteTest()
    {
        using ISymbolRepository<UITypeSymbol> repository = new UITypeSymbolRepository( new FilePath( Path.Combine( TestDataDirectory, "delete.json" ) ) );
        var callBack = CreateDummySymbol( "ui_button" );

        repository.Store( callBack );
        Assert.IsTrue( repository.Delete( callBack ) );
    }

    [Test]
    public void DeleteListTest()
    {
        using ISymbolRepository<UITypeSymbol> repository = new UITypeSymbolRepository( new FilePath( Path.Combine( TestDataDirectory, "delete_with_list.json" ) ) );
        var commands = new List<UITypeSymbol>()
        {
            CreateDummySymbol( "ui_button" ),
            CreateDummySymbol( "ui_label" ),
        };

        repository.Store( commands );
        Assert.IsTrue( repository.Delete( commands ) );
    }

    [Test]
    public void FindByNameTest()
    {
        using ISymbolRepository<UITypeSymbol> repository = new UITypeSymbolRepository( new FilePath( Path.Combine( TestDataDirectory, "find_by_name.json" ) ) );
        var commands = new List<UITypeSymbol>()
        {
            CreateDummySymbol( "ui_button" ),
            CreateDummySymbol( "ui_label" ),
        };

        repository.Store( commands );

        var found = repository.FindByName( "ui_button" );
        Assert.AreEqual( 1, found.Count() );

        found = repository.FindByName( "ui_button-" );
        Assert.AreEqual( 0, found.Count() );
    }

    [Test]
    public void FindTest()
    {
        using ISymbolRepository<UITypeSymbol> repository = new UITypeSymbolRepository( new FilePath( Path.Combine( TestDataDirectory, "find.json" ) ) );
        var commands = new List<UITypeSymbol>()
        {
            CreateDummySymbol( "ui_button" ),
            CreateDummySymbol( "ui_label" ),
        };

        repository.Store( commands );

        var found = repository.Find( x => x.Name == "ui_button" );
        Assert.AreEqual( 1, found.Count() );

        found = repository.Find( x => x.Name == "ui_button-" );
        Assert.AreEqual( 0, found.Count() );
    }

    [Test]
    public void FindAllTest()
    {
        using ISymbolRepository<UITypeSymbol> repository = new UITypeSymbolRepository( new FilePath( Path.Combine( TestDataDirectory, "find_all.json" ) ) );
        var commands = new List<UITypeSymbol>()
        {
            CreateDummySymbol( "ui_button" ),
            CreateDummySymbol( "ui_label" ),
        };

        repository.Store( commands );

        var found = repository.FindAll();
        Assert.AreEqual( 2, found.Count() );
    }
}
