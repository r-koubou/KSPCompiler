using System.IO;
using System.Linq;
using System.Threading.Tasks;

using KSPCompiler.Commons.Path;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbol.Commons;
using KSPCompiler.ExternalSymbol.Tsv.Callbacks;
using KSPCompiler.Gateways.Symbols;
using KSPCompiler.Infrastructures.Commons.LocalStorages;

using NUnit.Framework;

namespace KSPCompiler.ExternalSymbol.Tsv.Tests;

[TestFixture]
public class CallbackTableTsvLoaderTest
{
    private static readonly string TestDataDirectory = Path.Combine( "TestData", "CallbackTableTsvLoaderTest" );

    private static ISymbolImporter<CallbackSymbol> CreateLocalImporter( string path )
    {
        var reader = new LocalTextContentReader( new FilePath( path ) );
        return new TsvCallbackSymbolImporter( reader );
    }

    private static ISymbolExporter<CallbackSymbol> CreateLocalExporter( string path )
    {
        var writer = new LocalTextContentWriter( new FilePath( path ) );
        return new TsvCallbackSymbolExporter( writer );
    }

    [Test]
    public void TranslateSymbolTable()
    {
        var path = Path.Combine( TestDataDirectory, "CallbackTable.txt" );
        var importer = CreateLocalImporter( path );
        var symbolTable = importer.Import();

        Assert.That( symbolTable.Count, Is.EqualTo( 2 ) );

        var list = symbolTable.ToList();

        Assert.That( list[ 0 ].BuiltIntoVersion, Is.EqualTo( SymbolBuiltIntoVersion.NotAvailable ) );
        Assert.That( list[ 1 ].BuiltIntoVersion, Is.EqualTo( SymbolBuiltIntoVersion.NotAvailable ) );
    }

    [Test]
    public async Task TranslateSymbolTableAsync()
    {
        var path = Path.Combine( TestDataDirectory, "CallbackTable.txt" );
        var importer = CreateLocalImporter( path );

        await Task.Run( async () => {
            var symbolTable = await importer.ImportAsync();
            Assert.That( symbolTable.Count, Is.EqualTo( 2 ) );
        });
    }

    [Test]
    public void StoreTest()
    {
        var path = Path.Combine( TestDataDirectory, "CallbackTable.tsv" );
        var exporter = CreateLocalExporter( path );
        var symbols = MockSymbolTableUtility.CreateDummyCallbackSymbols();

        exporter.ExportAsync( symbols );
    }

    [Test]
    public async Task StoreAsyncTest()
    {
        var path = Path.Combine( TestDataDirectory, "CallbackTable.tsv" );
        var exporter = CreateLocalExporter( path );
        var symbolTable = MockSymbolTableUtility.CreateDummyCallbackSymbols();

        await exporter.ExportAsync( symbolTable );
    }
}
