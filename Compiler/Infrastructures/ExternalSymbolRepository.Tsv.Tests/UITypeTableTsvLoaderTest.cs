using System.IO;
using System.Threading.Tasks;

using KSPCompiler.Commons.Path;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Tests;
using KSPCompiler.ExternalSymbolRepository.Tsv.UITypes;
using KSPCompiler.Infrastructures.Commons.LocalStorages;

using NUnit.Framework;

namespace KSPCompiler.ExternalSymbolRepository.Tsv.Tests;

[TestFixture]
public class UITypeTableTsvLoaderTest
{
    private static readonly string TestDataDirectory = Path.Combine( "TestData", "UiTypeTableTsvLoaderTest" );

    private static ISymbolImporter<UITypeSymbol> CreateLocalImporter( string path )
    {
        var reader = new LocalTextContentReader( new FilePath( path ) );
        return new TsvUITypeSymbolImporter( reader );
    }

    private static ISymbolExporter<UITypeSymbol> CreateLocalExporter( string path )
    {
        var writer = new LocalTextContentWriter( new FilePath( path ) );
        return new TsvUITypeSymbolExporter( writer );
    }

    [Test]
    public void TranslateSymbolTable()
    {
        var path = Path.Combine( TestDataDirectory, "UiTypeTable.txt" );
        var importer = CreateLocalImporter( path );
        var symbolTable = importer.Import();

        Assert.IsTrue( symbolTable.Count == 1 );
    }

    [Test]
    public async Task TranslateSymbolTableAsync()
    {
        var path = Path.Combine( TestDataDirectory, "UiTypeTable.txt" );
        var importer = CreateLocalImporter( path );

        await Task.Run( async () => {
            var symbolTable = await importer.ImportAsync();
            Assert.IsTrue( symbolTable.Count == 1 );
        });
    }

    [Test]
    public void StoreTest()
    {
        var path = Path.Combine( TestDataDirectory, "UiTypeTable.tsv" );
        var exporter = CreateLocalExporter( path );
        var symbols = MockSymbolTableUtility.CreateDummyUITypeSymbols();

        exporter.ExportAsync( symbols );
    }

    [Test]
    public async Task StoreAsyncTest()
    {
        var path = Path.Combine( TestDataDirectory, "UiTypeTable.tsv" );
        var exporter = CreateLocalExporter( path );
        var symbolTable = MockSymbolTableUtility.CreateDummyUITypeSymbols();

        await exporter.ExportAsync( symbolTable );
    }
}
