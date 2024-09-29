using System.Collections.Generic;
using System.IO;
using System.Linq;

using KSPCompiler.Commons.Path;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Commands;

using NUnit.Framework;

namespace KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Tests;

[TestFixture]
public class CommandRepositoryTest
{
    private static readonly string TestDataDirectory = Path.Combine( "TestData", "CommandRepositoryTest" );

    [SetUp]
    public void Setup()
    {
        Directory.CreateDirectory( TestDataDirectory );
    }

    private static CommandSymbol CreateDummySymbol(string name)
    {
        var command = new CommandSymbol()
        {
            Name = name,
            Description = "Dummy command",
            DataType = DataTypeFlag.TypeVoid
        };

        command.AddArgument( new CommandArgumentSymbol
        {
            Name = "$arg1",
            Description = "Argument 1",
        });

        return command;
    }

    [Test]
    public void StoreTest()
    {
        using ISymbolRepository<CommandSymbol> repository = new CommandSymbolRepository( new FilePath( Path.Combine( TestDataDirectory, "store.json" ) ) );

        var command = CreateDummySymbol( "inc" );
        Assert.IsTrue( repository.Store( command ) );
    }

    [Test]
    public void StoreListTest()
    {
        using ISymbolRepository<CommandSymbol> repository = new CommandSymbolRepository( new FilePath( Path.Combine( TestDataDirectory, "store_with_list.json" ) ) );

        var commands = new List<CommandSymbol>()
        {
            CreateDummySymbol( "inc" ),
            CreateDummySymbol( "dec" ),
        };

        Assert.IsTrue( repository.Store( commands ) );
    }

    [Test]
    public void DeleteTest()
    {
        using ISymbolRepository<CommandSymbol> repository = new CommandSymbolRepository( new FilePath( Path.Combine( TestDataDirectory, "delete.json" ) ) );
        var callBack = CreateDummySymbol( "inc" );

        repository.Store( callBack );
        Assert.IsTrue( repository.Delete( callBack ) );
    }

    [Test]
    public void DeleteListTest()
    {
        using ISymbolRepository<CommandSymbol> repository = new CommandSymbolRepository( new FilePath( Path.Combine( TestDataDirectory, "delete_with_list.json" ) ) );
        var commands = new List<CommandSymbol>()
        {
            CreateDummySymbol( "inc" ),
            CreateDummySymbol( "dec" ),
        };

        repository.Store( commands );
        Assert.IsTrue( repository.Delete( commands ) );
    }

    [Test]
    public void FindByNameTest()
    {
        using ISymbolRepository<CommandSymbol> repository = new CommandSymbolRepository( new FilePath( Path.Combine( TestDataDirectory, "find_by_name.json" ) ) );
        var commands = new List<CommandSymbol>()
        {
            CreateDummySymbol( "inc" ),
            CreateDummySymbol( "dec" ),
        };

        repository.Store( commands );

        var found = repository.FindByName( "inc" );
        Assert.AreEqual( 1, found.Count() );

        found = repository.FindByName( "inc-" );
        Assert.AreEqual( 0, found.Count() );
    }

    [Test]
    public void FindTest()
    {
        using ISymbolRepository<CommandSymbol> repository = new CommandSymbolRepository( new FilePath( Path.Combine( TestDataDirectory, "find.json" ) ) );
        var commands = new List<CommandSymbol>()
        {
            CreateDummySymbol( "inc" ),
            CreateDummySymbol( "dec" ),
        };

        repository.Store( commands );

        var found = repository.Find( x => x.Name == "inc" );
        Assert.AreEqual( 1, found.Count() );

        found = repository.Find( x => x.Name == "inc-" );
        Assert.AreEqual( 0, found.Count() );
    }

    [Test]
    public void FindAllTest()
    {
        using ISymbolRepository<CommandSymbol> repository = new CommandSymbolRepository( new FilePath( Path.Combine( TestDataDirectory, "find_all.json" ) ) );
        var commands = new List<CommandSymbol>()
        {
            CreateDummySymbol( "inc" ),
            CreateDummySymbol( "dec" ),
        };

        repository.Store( commands );

        var found = repository.FindAll();
        Assert.AreEqual( 2, found.Count() );
    }
}
