using System.Collections.Generic;
using System.IO;
using System.Linq;

using KSPCompiler.Commons.Path;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.Repositories;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Callbacks;

using NUnit.Framework;
using NUnit.Framework.Legacy;

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
        var result = repository.Store( callBack );

        ClassicAssert.IsTrue( result.Success );
        ClassicAssert.IsTrue( result.Exception == null );
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

        var result = repository.Store( callBacks );

        ClassicAssert.IsTrue( result.Success );
        ClassicAssert.IsTrue( result.Exception == null );
    }

    [Test]
    public void DeleteTest()
    {
        using ISymbolRepository<CallbackSymbol> repository = new CallbackSymbolRepository( new FilePath( Path.Combine( TestDataDirectory, "delete.json" ) ) );
        var callBack = CreateDummySymbol( "init" );

        repository.Store( callBack );
        var result = repository.Delete( callBack );

        ClassicAssert.IsTrue( result.Success );
        ClassicAssert.IsTrue( result.Exception == null );
        ClassicAssert.IsTrue( result.DeletedCount == 1 );
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
        var result = repository.Delete( callBacks );

        ClassicAssert.IsTrue( result.Success );
        ClassicAssert.IsTrue( result.Exception == null );
        ClassicAssert.IsTrue( result.DeletedCount == 2 );
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
        ClassicAssert.AreEqual( 1, found.Count() );

        found = repository.FindByName( "init-" );
        ClassicAssert.AreEqual( 0, found.Count() );
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
        ClassicAssert.AreEqual( 1, found.Count() );

        found = repository.Find( x => x.Name == "init-" );
        ClassicAssert.AreEqual( 0, found.Count() );
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
        ClassicAssert.AreEqual( 2, found.Count() );
    }
}
