using System.Collections.Generic;
using System.IO;
using System.Linq;

using KSPCompiler.Commons.Path;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Domain.Symbols.Repositories;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Commands;

using NUnit.Framework;
using NUnit.Framework.Legacy;

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
        var result = repository.Store( command );

        Assert.That( result.Success, Is.True );
        Assert.That( result.Exception, Is.Null );
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

        var result = repository.Store( commands );

        Assert.That( result.Success, Is.True );
        Assert.That( result.Exception, Is.Null );
    }

    [Test]
    public void DeleteTest()
    {
        using ISymbolRepository<CommandSymbol> repository = new CommandSymbolRepository( new FilePath( Path.Combine( TestDataDirectory, "delete.json" ) ) );
        var command = CreateDummySymbol( "inc" );

        repository.Store( command );
        var result = repository.Delete( command );

        Assert.That( result.Success, Is.True );
        Assert.That( result.Exception, Is.Null );
        Assert.That( result.DeletedCount, Is.EqualTo( 1 ) );
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
        var result = repository.Delete( commands );

        Assert.That( result.Success, Is.True );
        Assert.That( result.Exception, Is.Null );
        Assert.That( result.DeletedCount, Is.EqualTo( 2 ) );
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
        Assert.That( found.Count, Is.EqualTo( 1 ) );

        found = repository.FindByName( "inc-" );
        Assert.That( found.Count, Is.EqualTo( 0 ) );
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
        Assert.That( found.Count, Is.EqualTo( 1 ) );

        found = repository.Find( x => x.Name == "inc-" );
        Assert.That( found.Count, Is.EqualTo( 0 ) );
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
        Assert.That( found.Count, Is.EqualTo( 2 ) );
    }
}
