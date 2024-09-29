using System;
using System.IO;

using JsonFlatFileDataStore;

using NUnit.Framework;

namespace KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Tests;

public class JsonFlatFileDataStoreDemo
{
    private static readonly string TestDataDirectory = Path.Combine( "TestData", "JsonFlatFileDataStoreDemo" );

    [SetUp]
    public void Setup()
    {
        Directory.CreateDirectory( TestDataDirectory );
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
