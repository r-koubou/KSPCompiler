using System.Collections.Generic;
using System.IO;
using System.Linq;

using KSPCompiler.Commons.Path;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
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
            DataType = DataTypeUtility.Guess( new SymbolName( name ) )
        };

        return variable;
    }

    [Test]
    public void StoreTest()
    {
        using ISymbolRepository<VariableSymbol> repository = new VariableSymbolRepository( new FilePath( Path.Combine( TestDataDirectory, "store.json" ) ) );

        var command = CreateDummySymbol( "$ENGINE_PAR_VOLUME" );
        Assert.IsTrue( repository.Store( command ) );
    }

    [Test]
    public void StoreListTest()
    {
        using ISymbolRepository<VariableSymbol> repository = new VariableSymbolRepository( new FilePath( Path.Combine( TestDataDirectory, "store_with_list.json" ) ) );

        var commands = new List<VariableSymbol>()
        {
            CreateDummySymbol( "$ENGINE_PAR_VOLUME" ),
            CreateDummySymbol( "$ENGINE_PAR_PAN" ),
        };

        Assert.IsTrue( repository.Store( commands ) );
    }

    [Test]
    public void DeleteTest()
    {
        using ISymbolRepository<VariableSymbol> repository = new VariableSymbolRepository( new FilePath( Path.Combine( TestDataDirectory, "delete.json" ) ) );
        var callBack = CreateDummySymbol( "$ENGINE_PAR_VOLUME" );

        repository.Store( callBack );
        Assert.IsTrue( repository.Delete( callBack ) );
    }

    [Test]
    public void DeleteListTest()
    {
        using ISymbolRepository<VariableSymbol> repository = new VariableSymbolRepository( new FilePath( Path.Combine( TestDataDirectory, "delete_with_list.json" ) ) );
        var commands = new List<VariableSymbol>()
        {
            CreateDummySymbol( "$ENGINE_PAR_VOLUME" ),
            CreateDummySymbol( "$ENGINE_PAR_PAN" ),
        };

        repository.Store( commands );
        Assert.IsTrue( repository.Delete( commands ) );
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
