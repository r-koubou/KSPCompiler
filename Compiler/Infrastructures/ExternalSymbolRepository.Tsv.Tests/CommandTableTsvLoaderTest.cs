using System.IO;
using System.Threading.Tasks;

using KSPCompiler.Commons.Path;
using KSPCompiler.Domain.Tests;
using KSPCompiler.ExternalSymbol.Commons;
using KSPCompiler.ExternalSymbolRepository.Tsv.Commands;
using KSPCompiler.Infrastructures.Commons.LocalStorages;
using KSPCompiler.UseCases.Symbols.Commons;

using NUnit.Framework;

namespace KSPCompiler.ExternalSymbolRepository.Tsv.Tests;

[TestFixture]
public class CommandTableTsvLoaderTest
{
    private static readonly string TestDataDirectory = Path.Combine( "TestData", "CommandTableTsvLoaderTest" );

    private static IExternalCommandSymbolImporter CreateLocalImporter( string path )
    {
        var reader = new LocalTextContentReader( new FilePath( path ) );
        return new TsvCommandSymbolImporter( reader );
    }

    private static IExternalCommandSymbolExporter CreateLocalExporter( string path )
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

        Assert.IsTrue( symbolTable.Table.Count == 1 );
    }

    [Test]
    public async Task TranslateSymbolTableAsync()
    {
        var path = Path.Combine( TestDataDirectory, "CommandTable.txt" );
        var importer = CreateLocalImporter( path );

        await Task.Run( async () => {
            var symbolTable = await importer.ImportAsync();
            Assert.IsTrue( symbolTable.Table.Count == 1 );
        });
    }

    [Test]
    public void CannotImportDuplicateSymbolTest()
    {
        var path = Path.Combine( TestDataDirectory, "DuplicateCommandTable.txt" );
        var importer = CreateLocalImporter( path );

        Assert.ThrowsAsync<DuplicatedSymbolException>( async () => await importer.ImportAsync() );
    }

    [Test]
    public void StoreTest()
    {
        var path = Path.Combine( TestDataDirectory, "CommandTable.tsv" );
        var exporter = CreateLocalExporter( path );
        var symbolTable = MockSymbolTableUtility.CreateDummyCommandSymbolTable();

        exporter.ExportAsync( symbolTable );
    }

    [Test]
    public async Task StoreAsyncTest()
    {
        var path = Path.Combine( TestDataDirectory, "CommandTable.tsv" );
        var exporter = CreateLocalExporter( path );
        var symbolTable = MockSymbolTableUtility.CreateDummyCommandSymbolTable();

        await exporter.ExportAsync( symbolTable );
    }
}
