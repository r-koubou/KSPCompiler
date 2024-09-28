using System;
using System.Collections.Generic;
using System.IO;

using JsonFlatFileDataStore;

using KSPCompiler.Commons.Path;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Callbacks;

using NUnit.Framework;

namespace KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Tests;

[TestFixture]
public class CallbackTableLoaderTest
{
    private static readonly string TestDataDirectory = Path.Combine( "TestData", "CallbackTableLoaderTest" );

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
    public void UseJsonFlatFileDataStoreDemo()
    {
        var jsonFilePath = Path.Combine( TestDataDirectory, "demo.json" );

        var record = new DemoRecord { id = Guid.NewGuid(), Name = "test" };
        var records = new[] { record };

        using var store = new DataStore( jsonFilePath );

        store.InsertItem( "version", "20240928" );
        store.InsertItem( "records", records );

        var items = store.GetCollection<DemoRecord>( "records" );

        if( items != null )
        {
            Console.Out.WriteLine($"items.Count: {items.Count}");

            foreach( var x in items.AsQueryable() )
            {
                Console.Out.WriteLine(x.Name);
            }
        }
        else
        {
            Console.Out.WriteLine("items is null");
        }

    }

    private class DemoRecord
    {
        public Guid id { get; set; }
        public string Name { get; set; } = "";
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Updated { get; set; } = DateTime.Now;
    }

}
