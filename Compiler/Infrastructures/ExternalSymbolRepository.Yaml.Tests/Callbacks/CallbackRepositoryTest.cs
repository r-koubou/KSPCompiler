using System.IO;

using KSPCompiler.Commons.Path;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.Repositories;
using KSPCompiler.ExternalSymbolRepository.Yaml.Callbacks;

using NUnit.Framework;

namespace KSPCompiler.ExternalSymbolRepository.Yaml.Tests.Callbacks;

[TestFixture]
public class CallbackRepositoryTest
{
    private static readonly string TestDataDirectory = MockUtility.GetTestDataDirectory( "Callbacks" );

    #region Find
    [Test]
    public void FindByNameTest()
    {
        var dbPath = Path.Combine( TestDataDirectory, "FindTest.yaml" );
        using ISymbolRepository<CallbackSymbol> repository = new CallbackSymbolRepository( new FilePath( dbPath ) );

        var symbols = repository.FindByName( "Test" );

        Assert.That( symbols.Count, Is.GreaterThan( 0 ) );
    }

    [Test]
    public void CannotFindByNotExistNameTest()
    {
        var dbPath = Path.Combine( TestDataDirectory, "FindTest.yaml" );
        using ISymbolRepository<CallbackSymbol> repository = new CallbackSymbolRepository( new FilePath( dbPath ) );

        var symbols = repository.FindByName( "hogehoge" );

        Assert.That( symbols.Count, Is.EqualTo( 0 ) );
    }

    [Test]
    public void FindAllTest()
    {
        var dbPath = Path.Combine( TestDataDirectory, "FindTest.yaml" );
        using ISymbolRepository<CallbackSymbol> repository = new CallbackSymbolRepository( new FilePath( dbPath ) );

        var symbol = repository.FindAll();

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

        using ISymbolRepository<CallbackSymbol> repository = new CallbackSymbolRepository( new FilePath( dbFilePath ), autoFlush: true );

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

        using ISymbolRepository<CallbackSymbol> repository = new CallbackSymbolRepository( new FilePath( dbFilePath ), autoFlush: true );

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

        using ISymbolRepository<CallbackSymbol> repository = new CallbackSymbolRepository( new FilePath( dbFilePath ), autoFlush: true );

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
