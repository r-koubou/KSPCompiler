using System.IO;
using System.Linq;
using System.Threading.Tasks;

using KSPCompiler.Features.Shared.IO.LocalStorages;
using KSPCompiler.Features.SymbolManagement.Gateways;
using KSPCompiler.Features.SymbolManagement.Infrastructures.Commons.Tests;
using KSPCompiler.Features.SymbolManagement.Infrastructures.ExternalSymbol.Tsv.Commands;
using KSPCompiler.Shared.Domain.Symbols;
using KSPCompiler.Shared.Domain.Symbols.MetaData;
using KSPCompiler.Shared.Path;

using NUnit.Framework;

namespace KSPCompiler.Features.SymbolManagement.Infrastructures.Tsv.Tests;

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

        Assert.That( symbolTable.Count, Is.EqualTo( 2 ) );

        var symbols = symbolTable.ToList();

        Assert.That( symbols[ 0 ].BuiltIntoVersion, Is.EqualTo( SymbolBuiltIntoVersion.NotAvailable ) );
        Assert.That( symbols[ 0 ].DataType, Is.EqualTo( DataTypeFlag.TypeVoid ) );
        Assert.That( symbols[ 0 ].Arguments.First().DataType, Is.EqualTo( DataTypeFlag.All ) );

        Assert.That( symbols[ 1 ].BuiltIntoVersion, Is.EqualTo( SymbolBuiltIntoVersion.NotAvailable ) );
        Assert.That( symbols[ 1 ].DataType, Is.EqualTo( DataTypeFlag.TypeVoid ) );
        Assert.That( symbols[ 1 ].Arguments.First().UITypeNames, Is.EqualTo( new[] { "ui_*" } ) );

    }

    [Test]
    public async Task TranslateSymbolTableAsync()
    {
        var path = Path.Combine( TestDataDirectory, "CommandTable.txt" );
        var importer = CreateLocalImporter( path );

        await Task.Run( async () => {
            var symbolTable = await importer.ImportAsync();
            Assert.That( symbolTable.Count, Is.EqualTo( 2 ) );
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
