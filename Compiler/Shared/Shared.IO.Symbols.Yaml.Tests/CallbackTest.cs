using System.Linq;
using System.Threading.Tasks;

using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Abstractions.Symbol;
using KSPCompiler.Shared.IO.Local;
using KSPCompiler.Shared.IO.Symbols.Mock;
using KSPCompiler.Shared.IO.Symbols.Yaml.Callbacks;
using KSPCompiler.Shared.Path;

using NUnit.Framework;

namespace KSPCompiler.Shared.IO.Symbols.Yaml.Tests;

[TestFixture]
public class CallbackTest
{
    private static readonly string TestDataDirectory = System.IO.Path.Combine( "TestData", "CallbackTest" );

    private static ISymbolImporter<CallbackSymbol> CreateLocalImporter( string path )
    {
        var reader = new LocalTextContentReader( new FilePath( path ) );
        return new YamlCallbackSymbolImporter( reader );
    }

    private static ISymbolExporter<CallbackSymbol> CreateLocalExporter( string path )
    {
        var writer = new LocalTextContentWriter( new FilePath( path ) );
        return new YamlCallbackSymbolExporter( writer );
    }

    [Test]
    public void TranslateSymbolTable()
    {
        var path = System.IO.Path.Combine( TestDataDirectory, "callback.yaml" );
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
        var path = System.IO.Path.Combine( TestDataDirectory, "callback.yaml" );
        var importer = CreateLocalImporter( path );

        await Task.Run( async () => {
            var symbolTable = await importer.ImportAsync();
            Assert.That( symbolTable.Count, Is.EqualTo( 2 ) );
        });
    }

    [Test]
    public void StoreTest()
    {
        var path = System.IO.Path.Combine( TestDataDirectory, "callback_out.yaml" );
        var exporter = CreateLocalExporter( path );
        var symbols = MockSymbolTableUtility.CreateDummyCallbackSymbols();

        exporter.ExportAsync( symbols );
    }

    [Test]
    public async Task StoreAsyncTest()
    {
        var path = System.IO.Path.Combine( TestDataDirectory, "callback_out.yaml" );
        var exporter = CreateLocalExporter( path );
        var symbolTable = MockSymbolTableUtility.CreateDummyCallbackSymbols();

        await exporter.ExportAsync( symbolTable );
    }
}
