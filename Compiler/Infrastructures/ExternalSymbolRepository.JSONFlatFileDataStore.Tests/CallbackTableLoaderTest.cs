using System;
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

    [Test]
    public void StoreTest()
    {
        using ISymbolRepository<CallbackSymbol> repository = new CallbackSymbolRepository( new FilePath( Path.Combine( TestDataDirectory, "callbacks.json" ) ) );

        var callBack = new CallbackSymbol( true )
        {
            Name = "init",
            Description = "Initialize callback",
        };

        callBack.AddArgument( new CallbackArgumentSymbol( false )
        {
            Name = "$arg1",
            Description = "Argument 1",
        });

        var result = repository.Store( callBack );
        Assert.IsTrue( result );
    }

    [Test]
    public void UseJsonFlatFileDataStoreTest()
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
