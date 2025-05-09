using System.Linq;
using System.Threading.Tasks;

using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Abstractions.Symbol;
using KSPCompiler.Shared.IO.Local;
using KSPCompiler.Shared.IO.Symbols.Mock;
using KSPCompiler.Shared.IO.Symbols.Yaml.UITypes;
using KSPCompiler.Shared.Path;

using NUnit.Framework;

namespace KSPCompiler.Shared.IO.Symbols.Yaml.Tests;

[TestFixture]
public class UITypeTest
{
    private static readonly string TestDataDirectory = System.IO.Path.Combine( "TestData", "UiTypeTest" );

    private static ISymbolImporter<UITypeSymbol> CreateLocalImporter( string path )
    {
        var reader = new LocalTextContentReader( new FilePath( path ) );
        return new YamlUITypeSymbolImporter( reader );
    }

    private static ISymbolExporter<UITypeSymbol> CreateLocalExporter( string path )
    {
        var writer = new LocalTextContentWriter( new FilePath( path ) );
        return new YamlUITypeSymbolExporter( writer );
    }

    [Test]
    public void TranslateSymbolTable()
    {
        var path = System.IO.Path.Combine( TestDataDirectory, "uitype.yaml" );
        var importer = CreateLocalImporter( path );
        var symbols = importer.Import();

        Assert.That( symbols.Count, Is.EqualTo( 2 ) );
        Assert.That( symbols.First().BuiltIntoVersion, Is.EqualTo( SymbolBuiltIntoVersion.NotAvailable ) );
    }

    [Test]
    public async Task TranslateSymbolTableAsync()
    {
        var path = System.IO.Path.Combine( TestDataDirectory, "uitype.yaml" );
        var importer = CreateLocalImporter( path );

        await Task.Run( async () => {
            var symbols = await importer.ImportAsync();
            Assert.That( symbols.Count, Is.EqualTo( 2 ) );
        });
    }

    [Test]
    public void StoreTest()
    {
        var path = System.IO.Path.Combine( TestDataDirectory, "uitype_out.yaml" );
        var exporter = CreateLocalExporter( path );
        var symbols = MockSymbolTableUtility.CreateDummyUITypeSymbols();

        exporter.ExportAsync( symbols );
    }

    [Test]
    public async Task StoreAsyncTest()
    {
        var path = System.IO.Path.Combine( TestDataDirectory, "uitype_out.yaml" );
        var exporter = CreateLocalExporter( path );
        var symbols = MockSymbolTableUtility.CreateDummyUITypeSymbols();

        await exporter.ExportAsync( symbols );
    }
}
