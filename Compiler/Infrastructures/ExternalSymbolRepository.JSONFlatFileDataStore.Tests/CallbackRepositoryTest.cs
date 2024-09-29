using System.Collections.Generic;
using System.IO;
using System.Linq;

using KSPCompiler.Commons.Path;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Callbacks;

using NUnit.Framework;

namespace KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Tests;

[TestFixture]
public class CallbackRepositoryTest
{
    private static readonly string TestDataDirectory = Path.Combine( "TestData", "CallbackRepositoryTest" );

    [SetUp]
    public void Setup()
    {
        Directory.CreateDirectory( TestDataDirectory );
    }

    private static CallbackSymbol CreateDummySymbol(string name)
    {
        var callback = new CallbackSymbol( true )
        {
            Name = name,
            Description = "Initialize callback",
        };

        callback.AddArgument( new CallbackArgumentSymbol( false )
        {
            Name = "$arg1",
            Description = "Argument 1",
        });

        return callback;
    }

    [Test]
    public void StoreTest()
    {
        using ISymbolRepository<CallbackSymbol> repository = new CallbackSymbolRepository( new FilePath( Path.Combine( TestDataDirectory, "store.json" ) ) );

        var callBack = CreateDummySymbol( "init" );
        Assert.IsTrue( repository.Store( callBack ) );
    }

    [Test]
    public void StoreListTest()
    {
        using ISymbolRepository<CallbackSymbol> repository = new CallbackSymbolRepository( new FilePath( Path.Combine( TestDataDirectory, "store_with_list.json" ) ) );

        var callBacks = new List<CallbackSymbol>()
        {
            CreateDummySymbol( "init" ),
            CreateDummySymbol( "start" ),
        };

        Assert.IsTrue( repository.Store( callBacks ) );
    }

    [Test]
    public void DeleteTest()
    {
        using ISymbolRepository<CallbackSymbol> repository = new CallbackSymbolRepository( new FilePath( Path.Combine( TestDataDirectory, "delete.json" ) ) );
        var callBack = CreateDummySymbol( "init" );

        repository.Store( callBack );
        Assert.IsTrue( repository.Delete( callBack ) );
    }

    [Test]
    public void DeleteListTest()
    {
        using ISymbolRepository<CallbackSymbol> repository = new CallbackSymbolRepository( new FilePath( Path.Combine( TestDataDirectory, "delete_with_list.json" ) ) );
        var callBacks = new List<CallbackSymbol>()
        {
            CreateDummySymbol( "init" ),
            CreateDummySymbol( "start" ),
        };

        repository.Store( callBacks );
        Assert.IsTrue( repository.Delete( callBacks ) );
    }

    [Test]
    public void FindByNameTest()
    {
        using ISymbolRepository<CallbackSymbol> repository = new CallbackSymbolRepository( new FilePath( Path.Combine( TestDataDirectory, "find_by_name.json" ) ) );
        var callBacks = new List<CallbackSymbol>()
        {
            CreateDummySymbol( "init" ),
            CreateDummySymbol( "start" ),
        };

        repository.Store( callBacks );

        var found = repository.FindByName( "init" );
        Assert.AreEqual( 1, found.Count() );

        found = repository.FindByName( "init-" );
        Assert.AreEqual( 0, found.Count() );
    }

    [Test]
    public void FindTest()
    {
        using ISymbolRepository<CallbackSymbol> repository = new CallbackSymbolRepository( new FilePath( Path.Combine( TestDataDirectory, "find.json" ) ) );
        var callBacks = new List<CallbackSymbol>()
        {
            CreateDummySymbol( "init" ),
            CreateDummySymbol( "start" ),
        };

        repository.Store( callBacks );

        var found = repository.Find( x => x.Name == "init" );
        Assert.AreEqual( 1, found.Count() );

        found = repository.Find( x => x.Name == "init-" );
        Assert.AreEqual( 0, found.Count() );
    }

    [Test]
    public void FindAllTest()
    {
        using ISymbolRepository<CallbackSymbol> repository = new CallbackSymbolRepository( new FilePath( Path.Combine( TestDataDirectory, "find_all.json" ) ) );
        var callBacks = new List<CallbackSymbol>()
        {
            CreateDummySymbol( "init" ),
            CreateDummySymbol( "start" ),
        };

        repository.Store( callBacks );

        var found = repository.FindAll();
        Assert.AreEqual( 2, found.Count() );
    }
}
