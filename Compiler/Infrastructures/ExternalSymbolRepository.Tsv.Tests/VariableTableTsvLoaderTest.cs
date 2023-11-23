using System.IO;
using System.Threading.Tasks;

using KSPCompiler.Commons.Path;
using KSPCompiler.Domain.Tests;
using KSPCompiler.ExternalSymbolRepository.Tsv.Variables;
using KSPCompiler.Infrastructures.Commons.LocalStorages;
using KSPCompiler.UseCases.Symbols.Commons;

using NUnit.Framework;

namespace KSPCompiler.ExternalSymbolRepository.Tsv.Tests;

[TestFixture]
public class VariableTableTsvLoaderTest
{
    private static readonly string TestDataDirectory = Path.Combine( "TestData", "VariableTableTsvLoaderTest" );

    private static IExternalVariableSymbolImporter CreateLocalImporter( string path )
    {
        var reader = new LocalTextContentReader( new FilePath( path ) );
        return new TsvVariableSymbolImporter( reader );
    }

    private static IExternalVariableSymbolExporter CreateLocalExporter( string path )
    {
        var writer = new LocalTextContentWriter( new FilePath( path ) );
        return new TsvVariableSymbolExporter( writer );
    }

    [Test]
    public void TranslateSymbolTable()
    {
        var path = Path.Combine( TestDataDirectory, "VariableTable.txt" );
        var repository = CreateLocalImporter( path );
        var symbolTable = repository.Import();

        Assert.IsTrue( symbolTable.Table.Count == 1 );
    }

    [Test]
    public async Task TranslateSymbolTableAsync()
    {
        var path = Path.Combine( TestDataDirectory, "VariableTable.txt" );
        var repository = CreateLocalImporter( path );

        await Task.Run( async () => {
            var symbolTable = await repository.ImportAsync();
            Assert.IsTrue( symbolTable.Table.Count == 1 );
        });
    }

    [Test]
    public void StoreTest()
    {
        var path = Path.Combine( TestDataDirectory, "VariableTable.tsv" );
        var repository = CreateLocalExporter( path );
        var symbolTable = MockSymbolTableUtility.CreateDummy();

        repository.ExportAsync( symbolTable );
    }

    [Test]
    public async Task StoreAsyncTest()
    {
        var path = Path.Combine( TestDataDirectory, "VariableTable.tsv" );
        var repository = CreateLocalExporter( path );
        var symbolTable = MockSymbolTableUtility.CreateDummy();

        await repository.ExportAsync( symbolTable );
    }
}
