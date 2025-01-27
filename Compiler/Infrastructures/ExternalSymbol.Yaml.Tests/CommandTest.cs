using System.IO;
using System.Linq;
using System.Threading.Tasks;

using KSPCompiler.Commons.Path;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.ExternalSymbol.Commons;
using KSPCompiler.ExternalSymbol.Yaml.Commands;
using KSPCompiler.Gateways.Symbols;
using KSPCompiler.Infrastructures.Commons.LocalStorages;

using NUnit.Framework;

namespace KSPCompiler.ExternalSymbol.Yaml.Tests;

[TestFixture]
public class CommandTest
{
    private static readonly string TestDataDirectory = Path.Combine( "TestData", "CommandTest" );

    private static ISymbolImporter<CommandSymbol> CreateLocalImporter( string path )
    {
        var reader = new LocalTextContentReader( new FilePath( path ) );

        return new CommandSymbolImporter( reader );
    }

    private static ISymbolExporter<CommandSymbol> CreateLocalExporter( string path )
    {
        var writer = new LocalTextContentWriter( new FilePath( path ) );

        return new CommandSymbolExporter( writer );
    }

    [Test]
    public void TranslateSymbolTable()
    {
        var path = Path.Combine( TestDataDirectory, "command.yaml" );
        var importer = CreateLocalImporter( path );
        var symbolTable = importer.Import();

        Assert.That( symbolTable.Count, Is.EqualTo( 2 ) );

        var symbols = symbolTable.ToList();

        Assert.That( symbols[ 0 ].BuiltIntoVersion,           Is.EqualTo( SymbolBuiltIntoVersion.NotAvailable ) );
        Assert.That( symbols[ 0 ].DataType,                   Is.EqualTo( DataTypeFlag.TypeString | DataTypeFlag.TypeInt | DataTypeFlag.TypeReal ) );
        Assert.That( symbols[ 0 ].Arguments.First().DataType, Is.EqualTo( DataTypeFlag.TypeInt | DataTypeFlag.TypeReal ) );

        Assert.That( symbols[ 1 ].BuiltIntoVersion,              Is.EqualTo( SymbolBuiltIntoVersion.NotAvailable ) );
        Assert.That( symbols[ 1 ].DataType,                      Is.EqualTo( DataTypeFlag.TypeVoid ) );
        Assert.That( symbols[ 1 ].Arguments.First().UITypeNames, Is.EqualTo( new[] { "ui_*" } ) );
    }

    [Test]
    public async Task TranslateSymbolTableAsync()
    {
        var path = Path.Combine( TestDataDirectory, "command.yaml" );
        var importer = CreateLocalImporter( path );

        await Task.Run( async () =>
            {
                var symbolTable = await importer.ImportAsync();
                Assert.That( symbolTable.Count, Is.EqualTo( 2 ) );
            }
        );
    }

    [Test]
    public void StoreTest()
    {
        var path = Path.Combine( TestDataDirectory, "command_out.yaml" );
        var exporter = CreateLocalExporter( path );
        var symbols = MockSymbolTableUtility.CreateDummyCommandSymbols();

        exporter.ExportAsync( symbols );
    }

    [Test]
    public async Task StoreAsyncTest()
    {
        var path = Path.Combine( TestDataDirectory, "command_out.yaml" );
        var exporter = CreateLocalExporter( path );
        var symbolTable = MockSymbolTableUtility.CreateDummyCommandSymbols();

        await exporter.ExportAsync( symbolTable );
    }
}
