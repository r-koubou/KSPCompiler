using System.Collections.Generic;
using System.IO;
using System.Linq;

using KSPCompiler.Commons.Path;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Domain.Symbols.Repositories;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Variables;

using NUnit.Framework;

namespace KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Tests;

[TestFixture]
public class VariableRepositoryTest
{
    private static readonly string TestDataDirectory = Path.Combine( "TestData", nameof( VariableRepositoryTest ) );

    [SetUp]
    public void Setup()
    {
        Directory.CreateDirectory( TestDataDirectory );
    }

    private static VariableSymbol CreateDummySymbol(string name)
    {
        var variable = new VariableSymbol
        {
            Name = name,
            Description = "Dummy variable",
            DataType = DataTypeUtility.GuessFromSymbolName( new SymbolName( name ) )
        };

        return variable;
    }

    [Test]
    public void StoreTest()
    {
        using ISymbolRepository<VariableSymbol> repository = new VariableSymbolRepository( new FilePath( Path.Combine( TestDataDirectory, "store.json" ) ) );

        var variable = CreateDummySymbol( "$ENGINE_PAR_VOLUME" );
        var result = repository.Store( variable );

        Assert.IsTrue( result.Success );
        Assert.IsTrue( result.Exception == null );
    }

    [Test]
    public void StoreListTest()
    {
        using ISymbolRepository<VariableSymbol> repository = new VariableSymbolRepository( new FilePath( Path.Combine( TestDataDirectory, "store_with_list.json" ) ) );

        var variables = new List<VariableSymbol>()
        {
            CreateDummySymbol( "$ENGINE_PAR_VOLUME" ),
            CreateDummySymbol( "$ENGINE_PAR_PAN" ),
        };

        var result = repository.Store( variables );

        Assert.IsTrue( result.Success );
        Assert.IsTrue( result.Exception == null );
    }

    [Test]
    public void DeleteTest()
    {
        using ISymbolRepository<VariableSymbol> repository = new VariableSymbolRepository( new FilePath( Path.Combine( TestDataDirectory, "delete.json" ) ) );
        var variable = CreateDummySymbol( "$ENGINE_PAR_VOLUME" );

        repository.Store( variable );
        var result = repository.Delete( variable );

        Assert.IsTrue( result.Success );
        Assert.IsTrue( result.Exception == null );
        Assert.IsTrue( result.DeletedCount == 1 );
    }

    [Test]
    public void DeleteListTest()
    {
        using ISymbolRepository<VariableSymbol> repository = new VariableSymbolRepository( new FilePath( Path.Combine( TestDataDirectory, "delete_with_list.json" ) ) );
        var variables = new List<VariableSymbol>()
        {
            CreateDummySymbol( "$ENGINE_PAR_VOLUME" ),
            CreateDummySymbol( "$ENGINE_PAR_PAN" ),
        };

        repository.Store( variables );
        var result = repository.Delete( variables );

        Assert.IsTrue( result.Success );
        Assert.IsTrue( result.Exception == null );
        Assert.IsTrue( result.DeletedCount == 2 );
    }

    [Test]
    public void FindByNameTest()
    {
        using ISymbolRepository<VariableSymbol> repository = new VariableSymbolRepository( new FilePath( Path.Combine( TestDataDirectory, "find_by_name.json" ) ) );
        var commands = new List<VariableSymbol>()
        {
            CreateDummySymbol( "$ENGINE_PAR_VOLUME" ),
            CreateDummySymbol( "$ENGINE_PAR_PAN" ),
        };

        repository.Store( commands );

        var found = repository.FindByName( "$ENGINE_PAR_VOLUME" );
        Assert.AreEqual( 1, found.Count() );

        found = repository.FindByName( "$ENGINE_PAR_VOLUME-" );
        Assert.AreEqual( 0, found.Count() );
    }

    [Test]
    public void FindTest()
    {
        using ISymbolRepository<VariableSymbol> repository = new VariableSymbolRepository( new FilePath( Path.Combine( TestDataDirectory, "find.json" ) ) );
        var commands = new List<VariableSymbol>()
        {
            CreateDummySymbol( "$ENGINE_PAR_VOLUME" ),
            CreateDummySymbol( "$ENGINE_PAR_PAN" ),
        };

        repository.Store( commands );

        var found = repository.Find( x => x.Name == "$ENGINE_PAR_VOLUME" );
        Assert.AreEqual( 1, found.Count() );

        found = repository.Find( x => x.Name == "$ENGINE_PAR_VOLUME-" );
        Assert.AreEqual( 0, found.Count() );
    }

    [Test]
    public void FindAllTest()
    {
        using ISymbolRepository<VariableSymbol> repository = new VariableSymbolRepository( new FilePath( Path.Combine( TestDataDirectory, "find_all.json" ) ) );
        var commands = new List<VariableSymbol>()
        {
            CreateDummySymbol( "$ENGINE_PAR_VOLUME" ),
            CreateDummySymbol( "$ENGINE_PAR_PAN" ),
        };

        repository.Store( commands );

        var found = repository.FindAll();
        Assert.AreEqual( 2, found.Count() );
    }
}
