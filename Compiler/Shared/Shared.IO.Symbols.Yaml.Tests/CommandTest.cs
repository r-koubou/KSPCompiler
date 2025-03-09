using System.Linq;
using System.Threading.Tasks;

using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData;
using KSPCompiler.Shared.IO.Abstractions.Symbol;
using KSPCompiler.Shared.IO.Local;
using KSPCompiler.Shared.IO.Symbols.Mock;
using KSPCompiler.Shared.IO.Symbols.Yaml.Commands;
using KSPCompiler.Shared.Path;

using NUnit.Framework;

namespace KSPCompiler.Shared.IO.Symbols.Yaml.Tests;

[TestFixture]
public class CommandTest
{
    private static readonly string TestDataDirectory = System.IO.Path.Combine( "TestData", "CommandTest" );

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
        var path = System.IO.Path.Combine( TestDataDirectory, "command.yaml" );
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
        var path = System.IO.Path.Combine( TestDataDirectory, "command.yaml" );
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
        var path = System.IO.Path.Combine( TestDataDirectory, "command_out.yaml" );
        var exporter = CreateLocalExporter( path );
        var symbols = MockSymbolTableUtility.CreateDummyCommandSymbols();

        exporter.ExportAsync( symbols );
    }

    [Test]
    public async Task StoreAsyncTest()
    {
        var path = System.IO.Path.Combine( TestDataDirectory, "command_out.yaml" );
        var exporter = CreateLocalExporter( path );
        var symbolTable = MockSymbolTableUtility.CreateDummyCommandSymbols();

        await exporter.ExportAsync( symbolTable );
    }
}
