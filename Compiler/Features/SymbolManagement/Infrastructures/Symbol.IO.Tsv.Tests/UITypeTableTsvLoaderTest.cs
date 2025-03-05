using System.IO;
using System.Linq;
using System.Threading.Tasks;

using KSPCompiler.Features.Shared.IO.LocalStorages;
using KSPCompiler.Features.SymbolManagement.Gateways;
using KSPCompiler.Features.SymbolManagement.Infrastructures.Commons.Tests;
using KSPCompiler.Features.SymbolManagement.Infrastructures.Symbol.IO.Tsv.UITypes;
using KSPCompiler.Shared.Domain.Symbols;
using KSPCompiler.Shared.Path;

using NUnit.Framework;

namespace KSPCompiler.Features.SymbolManagement.Infrastructures.Symbol.IO.Tsv.Tests;

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
        var symbols = importer.Import();

        Assert.That( symbols.Count, Is.EqualTo( 1 ) );
        Assert.That( symbols.First().BuiltIntoVersion, Is.EqualTo( SymbolBuiltIntoVersion.NotAvailable ) );
    }

    [Test]
    public async Task TranslateSymbolTableAsync()
    {
        var path = Path.Combine( TestDataDirectory, "UiTypeTable.txt" );
        var importer = CreateLocalImporter( path );

        await Task.Run( async () => {
            var symbols = await importer.ImportAsync();
            Assert.That( symbols.Count, Is.EqualTo( 1 ) );
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
        var symbols = MockSymbolTableUtility.CreateDummyUITypeSymbols();

        await exporter.ExportAsync( symbols );
    }
}
