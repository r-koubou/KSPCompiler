using System.IO;
using System.Linq;
using System.Threading.Tasks;

using KSPCompiler.Commons.Contents;
using KSPCompiler.Commons.Path;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.Repositories;
using KSPCompiler.ExternalSymbol.Tsv.Callbacks;
using KSPCompiler.ExternalSymbol.Tsv.Commands;
using KSPCompiler.ExternalSymbol.Tsv.UITypes;
using KSPCompiler.ExternalSymbol.Tsv.Variables;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Callbacks;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Commands;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.UITypes;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Variables;
using KSPCompiler.Infrastructures.Commons.LocalStorages;

using NUnit.Framework;

namespace KSPCompiler.SymbolDatabaseControllers.Tests;

[TestFixture]
public class SymbolLocalDatabaseControllerTest
{
    private static readonly string TestDataDirectory = Path.Combine( "TestData",              "SymbolLocalDatabaseControllerTest" );
    private static readonly string ImportTestDataDirectory = Path.Combine( TestDataDirectory, "import" );
    private static readonly string ExportTestDataDirectory = Path.Combine( TestDataDirectory, "export" );
    private static readonly string DeleteTestDataDirectory = Path.Combine( TestDataDirectory, "delete" );
    private static readonly string FindTestDataDirectory = Path.Combine( TestDataDirectory,   "find" );

    [SetUp]
    public void Setup()
    {
        Directory.CreateDirectory( TestDataDirectory );
    }

    #region Variable

    [Test]
    public async Task ImportVariablesTest()
    {
        var importPath = new FilePath( Path.Combine( ImportTestDataDirectory,     "variable.tsv" ) );
        var repositoryPath = new FilePath( Path.Combine( ImportTestDataDirectory, "repository_variable.json" ) );

        ITextContentReader reader = new LocalTextContentReader( importPath );
        ISymbolImporter<VariableSymbol> importer = new TsvVariableSymbolImporter( reader );
        using ISymbolRepository<VariableSymbol> repository = new VariableSymbolRepository( repositoryPath );
        var controller = new SymbolDatabaseController<VariableSymbol>( repository );

        var result = await controller.ImportAsync( importer );

        Assert.IsTrue( result.Success );
    }

    [Test]
    public async Task ExportVariablesTest()
    {
        var exportPath = new FilePath( Path.Combine( ExportTestDataDirectory,     "variable.tsv" ) );
        var repositoryPath = new FilePath( Path.Combine( ExportTestDataDirectory, "repository_variable.json" ) );

        ITextContentWriter writer = new LocalTextContentWriter( exportPath );
        ISymbolExporter<VariableSymbol> exporter = new TsvVariableSymbolExporter( writer );
        using ISymbolRepository<VariableSymbol> repository = new VariableSymbolRepository( repositoryPath );
        var controller = new SymbolDatabaseController<VariableSymbol>( repository );

        var result = await controller.ExportAsync( exporter, _ => true );

        Assert.IsTrue( result.Success );
    }

    [Test]
    public async Task DeleteVariablesTest()
    {
        var repositoryPath = new FilePath( Path.Combine( DeleteTestDataDirectory, "repository_variable.json" ) );

        using ISymbolRepository<VariableSymbol> repository = new VariableSymbolRepository( repositoryPath );
        var controller = new SymbolDatabaseController<VariableSymbol>( repository );

        var result = await controller.DeleteAsync( _ => true );

        Assert.IsTrue( result.Success );
        Assert.IsFalse( (await repository.FindAllAsync()).Any() );
    }

    [Test]
    public async Task FindVariablesTest()
    {
        var repositoryPath = new FilePath( Path.Combine( FindTestDataDirectory, "repository_variable.json" ) );

        using ISymbolRepository<VariableSymbol> repository = new VariableSymbolRepository( repositoryPath );
        var controller = new SymbolDatabaseController<VariableSymbol>( repository );

        var result = await controller.FindAsync( _ => true );

        Assert.IsTrue( result.Success );
        Assert.IsTrue( result.FoundSymbols.Any() );
    }

    #endregion

    #region Command

    [Test]
    public async Task ImportCommandsTest()
    {
        var importPath = new FilePath( Path.Combine( ImportTestDataDirectory,     "command.tsv" ) );
        var repositoryPath = new FilePath( Path.Combine( ImportTestDataDirectory, "repository_command.json" ) );

        ITextContentReader reader = new LocalTextContentReader( importPath );
        ISymbolImporter<CommandSymbol> importer = new TsvCommandSymbolImporter( reader );
        using ISymbolRepository<CommandSymbol> repository = new CommandSymbolRepository( repositoryPath );
        var controller = new SymbolDatabaseController<CommandSymbol>( repository );

        var result = await controller.ImportAsync( importer );

        Assert.IsTrue( result.Success );
    }

    [Test]
    public async Task ExportCommandsTest()
    {
        var exportPath = new FilePath( Path.Combine( ExportTestDataDirectory,     "command.tsv" ) );
        var repositoryPath = new FilePath( Path.Combine( ExportTestDataDirectory, "repository_command.json" ) );

        ITextContentWriter writer = new LocalTextContentWriter( exportPath );
        ISymbolExporter<CommandSymbol> exporter = new TsvCommandSymbolExporter( writer );
        using ISymbolRepository<CommandSymbol> repository = new CommandSymbolRepository( repositoryPath );
        var controller = new SymbolDatabaseController<CommandSymbol>( repository );

        var result = await controller.ExportAsync( exporter, _ => true );

        Assert.IsTrue( result.Success );
    }

    [Test]
    public async Task DeleteCommandsTest()
    {
        var repositoryPath = new FilePath( Path.Combine( DeleteTestDataDirectory, "repository_command.json" ) );

        using ISymbolRepository<CommandSymbol> repository = new CommandSymbolRepository( repositoryPath );
        var controller = new SymbolDatabaseController<CommandSymbol>( repository );

        var result = await controller.DeleteAsync( _ => true );

        Assert.IsTrue( result.Success );
        Assert.IsFalse( ( await repository.FindAllAsync() ).Any() );
    }

    [Test]
    public async Task FindCommandsTest()
    {
        var repositoryPath = new FilePath( Path.Combine( FindTestDataDirectory, "repository_command.json" ) );

        using ISymbolRepository<CommandSymbol> repository = new CommandSymbolRepository( repositoryPath );
        var controller = new SymbolDatabaseController<CommandSymbol>( repository );

        var result = await controller.FindAsync( _ => true );

        Assert.IsTrue( result.Success );
        Assert.IsTrue( result.FoundSymbols.Any() );
    }

    #endregion

    #region Callback

    [Test]
    public async Task ImportCallbacksTest()
    {
        var importPath = new FilePath( Path.Combine( ImportTestDataDirectory,     "callback.tsv" ) );
        var repositoryPath = new FilePath( Path.Combine( ImportTestDataDirectory, "repository_callback.json" ) );

        ITextContentReader reader = new LocalTextContentReader( importPath );
        ISymbolImporter<CallbackSymbol> importer = new TsvCallbackSymbolImporter( reader );
        using ISymbolRepository<CallbackSymbol> repository = new CallbackSymbolRepository( repositoryPath );
        var controller = new SymbolDatabaseController<CallbackSymbol>( repository );

        var result = await controller.ImportAsync( importer );

        Assert.IsTrue( result.Success );
    }

    [Test]
    public async Task ExportCallbacksTest()
    {
        var exportPath = new FilePath( Path.Combine( ExportTestDataDirectory,     "callback.tsv" ) );
        var repositoryPath = new FilePath( Path.Combine( ExportTestDataDirectory, "repository_callback.json" ) );

        ITextContentWriter writer = new LocalTextContentWriter( exportPath );
        ISymbolExporter<CallbackSymbol> exporter = new TsvCallbackSymbolExporter( writer );
        using ISymbolRepository<CallbackSymbol> repository = new CallbackSymbolRepository( repositoryPath );
        var controller = new SymbolDatabaseController<CallbackSymbol>( repository );

        var result = await controller.ExportAsync( exporter, _ => true );

        Assert.IsTrue( result.Success );
    }

    [Test]
    public async Task DeleteCallbacksTest()
    {
        var repositoryPath = new FilePath( Path.Combine( DeleteTestDataDirectory, "repository_callback.json" ) );

        using ISymbolRepository<CallbackSymbol> repository = new CallbackSymbolRepository( repositoryPath );
        var controller = new SymbolDatabaseController<CallbackSymbol>( repository );

        var result = await controller.DeleteAsync( _ => true );

        Assert.IsTrue( result.Success );
        Assert.IsFalse( ( await repository.FindAllAsync() ).Any() );
    }

    [Test]
    public async Task FindCallbacksTest()
    {
        var repositoryPath = new FilePath( Path.Combine( FindTestDataDirectory, "repository_callback.json" ) );

        using ISymbolRepository<CallbackSymbol> repository = new CallbackSymbolRepository( repositoryPath );
        var controller = new SymbolDatabaseController<CallbackSymbol>( repository );

        var result = await controller.FindAsync( _ => true );

        Assert.IsTrue( result.Success );
        Assert.IsTrue( result.FoundSymbols.Any() );
    }

    #endregion

    #region UIType

    [Test]
    public async Task ImportUITypeTest()
    {
        var importPath = new FilePath( Path.Combine( ImportTestDataDirectory,     "ui_type.tsv" ) );
        var repositoryPath = new FilePath( Path.Combine( ImportTestDataDirectory, "repository_ui_type.json" ) );

        ITextContentReader reader = new LocalTextContentReader( importPath );
        ISymbolImporter<UITypeSymbol> importer = new TsvUITypeSymbolImporter( reader );
        using ISymbolRepository<UITypeSymbol> repository = new UITypeSymbolRepository( repositoryPath );
        var controller = new SymbolDatabaseController<UITypeSymbol>( repository );

        var result = await controller.ImportAsync( importer );

        Assert.IsTrue( result.Success );
    }

    [Test]
    public async Task ExportUITypeTest()
    {
        var exportPath = new FilePath( Path.Combine( ExportTestDataDirectory,     "ui_type.tsv" ) );
        var repositoryPath = new FilePath( Path.Combine( ExportTestDataDirectory, "repository_ui_type.json" ) );

        ITextContentWriter writer = new LocalTextContentWriter( exportPath );
        ISymbolExporter<UITypeSymbol> exporter = new TsvUITypeSymbolExporter( writer );
        using ISymbolRepository<UITypeSymbol> repository = new UITypeSymbolRepository( repositoryPath );
        var controller = new SymbolDatabaseController<UITypeSymbol>( repository );

        var result = await controller.ExportAsync( exporter, _ => true );

        Assert.IsTrue( result.Success );
    }

    [Test]
    public async Task DeleteUITypeTest()
    {
        var repositoryPath = new FilePath( Path.Combine( DeleteTestDataDirectory, "repository_ui_type.json" ) );

        using ISymbolRepository<UITypeSymbol> repository = new UITypeSymbolRepository( repositoryPath );
        var controller = new SymbolDatabaseController<UITypeSymbol>( repository );

        var result = await controller.DeleteAsync( _ => true );

        Assert.IsTrue( result.Success );
        Assert.IsFalse( ( await repository.FindAllAsync() ).Any() );
    }

    [Test]
    public async Task FindUITypeTest()
    {
        var repositoryPath = new FilePath( Path.Combine( FindTestDataDirectory, "repository_ui_type.json" ) );

        using ISymbolRepository<UITypeSymbol> repository = new UITypeSymbolRepository( repositoryPath );
        var controller = new SymbolDatabaseController<UITypeSymbol>( repository );

        var result = await controller.FindAsync( _ => true );

        Assert.IsTrue( result.Success );
        Assert.IsTrue( result.FoundSymbols.Any() );
    }

    #endregion
}
