using System.IO;

using KSPCompiler.Features.SymbolManagement.Gateways;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Local;
using KSPCompiler.Shared.IO.Symbols.Yaml.Callbacks;
using KSPCompiler.SymbolManagement.Repository.Yaml;

using NUnit.Framework;

namespace KSPCompiler.Features.SymbolManagement.Infrastructures.SymbolRepository.Yaml.Tests.Callbacks;

[TestFixture]
public class CallbackRepositoryTest
{
    private static readonly string TestDataDirectory = MockUtility.GetTestDataDirectory( "Callbacks" );

    #region Find
    [Test]
    public void FindByNameTest()
    {
        var dbPath = Path.Combine( TestDataDirectory, "FindTest.yaml" );
        var importer = new YamlCallbackSymbolImporter( new LocalTextContentReader( dbPath ) );
        using ISymbolRepository<CallbackSymbol> repository = new CallbackSymbolRepository( repositoryReader: importer );

        var symbols = repository.FindByName( "Test" );

        Assert.That( symbols.Count, Is.GreaterThan( 0 ) );
    }

    [Test]
    public void CannotFindByNotExistNameTest()
    {
        var dbPath = Path.Combine( TestDataDirectory, "FindTest.yaml" );
        var importer = new YamlCallbackSymbolImporter( new LocalTextContentReader( dbPath ) );
        using ISymbolRepository<CallbackSymbol> repository = new CallbackSymbolRepository( repositoryReader: importer );

        var symbols = repository.FindByName( "hogehoge" );

        Assert.That( symbols.Count, Is.EqualTo( 0 ) );
    }

    [Test]
    public void FindAllTest()
    {
        var dbPath = Path.Combine( TestDataDirectory, "FindTest.yaml" );
        var importer = new YamlCallbackSymbolImporter( new LocalTextContentReader( dbPath ) );
        using ISymbolRepository<CallbackSymbol> repository = new CallbackSymbolRepository( repositoryReader: importer );

        var symbol = repository.ToList();

        Assert.That( symbol.Count, Is.GreaterThan( 0 ) );
        Assert.That( symbol.Count, Is.EqualTo( repository.Count ) );
    }
    #endregion ~Find

    #region Store
    private static void CleanUp( string filePath )
    {
        if( File.Exists( filePath ) )
        {
            File.Delete( filePath );
        }
    }

    [Test]
    public void StoreTest()
    {
        var dbFilePath = Path.Combine( TestDataDirectory, "StoreTest.yaml" );
        CleanUp( dbFilePath );

        var exporter = new YamlCallbackSymbolExporter( new LocalTextContentWriter( dbFilePath ) );
        using ISymbolRepository<CallbackSymbol> repository = new CallbackSymbolRepository( repositoryWriter: exporter, autoFlush: true );

        var symbol = MockUtility.CreateCallbackSymbolModel( "test" );
        var result = repository.Store( symbol );

        Assert.That( result.Success,      Is.True );
        Assert.That( result.CreatedCount, Is.EqualTo( 1 ) );
        Assert.That( repository.Count,    Is.EqualTo( 1 ) );

        symbol = MockUtility.CreateCallbackSymbolModel( "hoge" );
        result = repository.Store( symbol );

        Assert.That( result.Success,   Is.True );
        Assert.That( repository.Count, Is.EqualTo( 2 ) );
    }

    [Test]
    public void StoreWithUpdateTest()
    {
        var dbFilePath = Path.Combine( TestDataDirectory, "StoreTest.yaml" );
        CleanUp( dbFilePath );

        var exporter = new YamlCallbackSymbolExporter( new LocalTextContentWriter( dbFilePath ) );
        using ISymbolRepository<CallbackSymbol> repository = new CallbackSymbolRepository( repositoryWriter: exporter, autoFlush: true );

        var symbol = MockUtility.CreateCallbackSymbolModel( "test" );
        var result = repository.Store( symbol );

        Assert.That( result.Success,   Is.True );
        Assert.That( repository.Count, Is.EqualTo( 1 ) );

        symbol.Description = "Updated Description";
        result             = repository.Store( symbol );

        Assert.That( result.Success,      Is.True );
        Assert.That( result.UpdatedCount, Is.EqualTo( 1 ) );
        Assert.That( repository.Count,    Is.EqualTo( 1 ) );
    }
    #endregion ~Store

    #region Delete
    [Test]
    public void DeleteTest()
    {
        var dbFilePath = Path.Combine( TestDataDirectory, "DeleteTest.yaml" );
        CleanUp( dbFilePath );

        var exporter = new YamlCallbackSymbolExporter( new LocalTextContentWriter( dbFilePath ) );
        using ISymbolRepository<CallbackSymbol> repository = new CallbackSymbolRepository( repositoryWriter: exporter, autoFlush: true );

        var symbol = MockUtility.CreateCallbackSymbolModel( "test" );
        var result = repository.Store( symbol );

        Assert.That( result.Success,      Is.True );
        Assert.That( result.CreatedCount, Is.EqualTo( 1 ) );
        Assert.That( repository.Count,    Is.EqualTo( 1 ) );

        var deleteResult = repository.Delete( symbol );

        Assert.That( deleteResult.Success,      Is.True );
        Assert.That( deleteResult.DeletedCount, Is.EqualTo( 1 ) );
        Assert.That( repository.Count,          Is.EqualTo( 0 ) );
    }
    #endregion ~Delete
}
