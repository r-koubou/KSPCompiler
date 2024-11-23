using System.IO;
using System.Linq;
using System.Threading.Tasks;

using KSPCompiler.Commons.Path;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.ExternalSymbol.Commons;
using KSPCompiler.ExternalSymbol.Tsv.Commands;
using KSPCompiler.Infrastructures.Commons.LocalStorages;

using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace KSPCompiler.ExternalSymbol.Tsv.Tests;

[TestFixture]
public class CommandTableTsvLoaderTest
{
    private static readonly string TestDataDirectory = Path.Combine( "TestData", "CommandTableTsvLoaderTest" );

    private static ISymbolImporter<CommandSymbol> CreateLocalImporter( string path )
    {
        var reader = new LocalTextContentReader( new FilePath( path ) );
        return new TsvCommandSymbolImporter( reader );
    }

    private static ISymbolExporter<CommandSymbol> CreateLocalExporter( string path )
    {
        var writer = new LocalTextContentWriter( new FilePath( path ) );
        return new TsvCommandSymbolExporter( writer );
    }

    [Test]
    public void TranslateSymbolTable()
    {
        var path = Path.Combine( TestDataDirectory, "CommandTable.txt" );
        var importer = CreateLocalImporter( path );
        var symbolTable = importer.Import();

        ClassicAssert.IsTrue( symbolTable.Count == 2 );

        var symbols = symbolTable.ToList();
        ClassicAssert.AreEqual( SymbolBuiltIntoVersion.NotAvailable, symbols[ 0 ].BuiltIntoVersion );
        ClassicAssert.AreEqual( DataTypeFlag.TypeVoid,               symbols[ 0 ].DataType );
        ClassicAssert.AreEqual( DataTypeFlag.MultipleType,           symbols[ 0 ].Arguments.First().DataType );

        ClassicAssert.AreEqual( SymbolBuiltIntoVersion.NotAvailable, symbols[ 1 ].BuiltIntoVersion );
        ClassicAssert.AreEqual( DataTypeFlag.TypeVoid,               symbols[ 1 ].DataType );
        ClassicAssert.AreEqual( new string[] { "ui_*" },             symbols[ 1 ].Arguments.First().UITypeNames );
    }

    [Test]
    public async Task TranslateSymbolTableAsync()
    {
        var path = Path.Combine( TestDataDirectory, "CommandTable.txt" );
        var importer = CreateLocalImporter( path );

        await Task.Run( async () => {
            var symbolTable = await importer.ImportAsync();
            ClassicAssert.IsTrue( symbolTable.Count == 2 );
        });
    }

    [Test]
    public void StoreTest()
    {
        var path = Path.Combine( TestDataDirectory, "CommandTable.tsv" );
        var exporter = CreateLocalExporter( path );
        var symbols = MockSymbolTableUtility.CreateDummyCommandSymbols();

        exporter.ExportAsync( symbols );
    }

    [Test]
    public async Task StoreAsyncTest()
    {
        var path = Path.Combine( TestDataDirectory, "CommandTable.tsv" );
        var exporter = CreateLocalExporter( path );
        var symbolTable = MockSymbolTableUtility.CreateDummyCommandSymbols();

        await exporter.ExportAsync( symbolTable );
    }
}
