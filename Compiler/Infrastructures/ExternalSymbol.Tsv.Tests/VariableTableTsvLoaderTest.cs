using System.IO;
using System.Linq;
using System.Threading.Tasks;

using KSPCompiler.Commons.Path;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbol.Commons;
using KSPCompiler.ExternalSymbol.Tsv.Variables;
using KSPCompiler.Infrastructures.Commons.LocalStorages;

using NUnit.Framework;

namespace KSPCompiler.ExternalSymbol.Tsv.Tests;

[TestFixture]
public class VariableTableTsvLoaderTest
{
    private static readonly string TestDataDirectory = Path.Combine( "TestData", "VariableTableTsvLoaderTest" );

    private static ISymbolImporter<VariableSymbol> CreateLocalImporter( string path )
    {
        var reader = new LocalTextContentReader( new FilePath( path ) );
        return new TsvVariableSymbolImporter( reader );
    }

    private static ISymbolExporter<VariableSymbol> CreateLocalExporter( string path )
    {
        var writer = new LocalTextContentWriter( new FilePath( path ) );
        return new TsvVariableSymbolExporter( writer );
    }

    [Test]
    public void TranslateSymbolTable()
    {
        var path = Path.Combine( TestDataDirectory, "VariableTable.txt" );
        var importer = CreateLocalImporter( path );
        var symbols = importer.Import();

        Assert.That( symbols.Count, Is.EqualTo( 1 ) );
        Assert.That( symbols.First().BuiltIntoVersion, Is.EqualTo( SymbolBuiltIntoVersion.NotAvailable ) );
    }

    [Test]
    public async Task TranslateSymbolTableAsync()
    {
        var path = Path.Combine( TestDataDirectory, "VariableTable.txt" );
        var importer = CreateLocalImporter( path );

        await Task.Run( async () => {
            var symbols = await importer.ImportAsync();
            Assert.That( symbols.Count, Is.EqualTo( 1 ) );
        });
    }

    [Test]
    public void StoreTest()
    {
        var path = Path.Combine( TestDataDirectory, "VariableTable.tsv" );
        var exporter = CreateLocalExporter( path );
        var symbolTable = MockSymbolTableUtility.CreateDummyVariableSymbols();

        exporter.ExportAsync( symbolTable );
    }

    [Test]
    public async Task StoreAsyncTest()
    {
        var path = Path.Combine( TestDataDirectory, "VariableTable.tsv" );
        var exporter = CreateLocalExporter( path );
        var symbolTable = MockSymbolTableUtility.CreateDummyVariableSymbols();

        await exporter.ExportAsync( symbolTable );
    }
}
