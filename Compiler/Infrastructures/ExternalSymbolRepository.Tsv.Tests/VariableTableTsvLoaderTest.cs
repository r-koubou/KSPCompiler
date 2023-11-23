using System.IO;
using System.Threading.Tasks;

using KSPCompiler.Commons.Path;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Tests;
using KSPCompiler.ExternalSymbolRepository.Tsv.Variables;
using KSPCompiler.Infrastructures.Commons.LocalStorages;

using NUnit.Framework;

namespace KSPCompiler.ExternalSymbolRepository.Tsv.Tests;

[TestFixture]
public class VariableTableTsvLoaderTest
{
    private static readonly string TestDataDirectory = Path.Combine( "TestData", "VariableTableTsvLoaderTest" );

    private static IVariableSymbolRepository CreateLocalRepository( string path )
    {
        var reader = new LocalTextContentReader( new FilePath( path ) );
        var writer = new LocalTextContentWriter( new FilePath( path ) );

        return new TsvVariableSymbolRepository( reader, writer );
    }

    [Test]
    public void TranslateSymbolTable()
    {
        var path = Path.Combine( TestDataDirectory, "VariableTable.txt" );
        var repository = CreateLocalRepository( path );
        var symbolTable = repository.Load();

        Assert.IsTrue( symbolTable.Table.Count == 1 );
    }

    [Test]
    public async Task TranslateSymbolTableAsync()
    {
        var path = Path.Combine( TestDataDirectory, "VariableTable.txt" );
        var repository = CreateLocalRepository( path );

        await Task.Run( async () => {
            var symbolTable = await repository.LoadAsync();
            Assert.IsTrue( symbolTable.Table.Count == 1 );
        });
    }

    [Test]
    public void StoreTest()
    {
        var path = Path.Combine( TestDataDirectory, "VariableTable.tsv" );
        var repository = CreateLocalRepository( path );
        var symbolTable = MockSymbolTableUtility.CreateDummy();

        repository.Store( symbolTable );
    }

    [Test]
    public async Task StoreAsyncTest()
    {
        var path = Path.Combine( TestDataDirectory, "VariableTable.tsv" );
        var repository = CreateLocalRepository( path );
        var symbolTable = MockSymbolTableUtility.CreateDummy();

        await repository.StoreAsync( symbolTable );
    }
}
