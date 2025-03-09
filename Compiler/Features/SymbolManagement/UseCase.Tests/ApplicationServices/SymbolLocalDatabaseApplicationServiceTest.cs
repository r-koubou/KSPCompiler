using System.IO;
using System.Linq;
using System.Threading.Tasks;

using KSPCompiler.Features.SymbolManagement.Gateways;
using KSPCompiler.Features.SymbolManagement.Infrastructures.Symbol.IO.Tsv.Callbacks;
using KSPCompiler.Features.SymbolManagement.Infrastructures.Symbol.IO.Tsv.Commands;
using KSPCompiler.Features.SymbolManagement.Infrastructures.Symbol.IO.Tsv.UITypes;
using KSPCompiler.Features.SymbolManagement.Infrastructures.Symbol.IO.Tsv.Variables;
using KSPCompiler.Features.SymbolManagement.UseCase.ApplicationServices;
using KSPCompiler.Shared.Contents;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Abstractions.LocalStorages;
using KSPCompiler.Shared.Path;
using KSPCompiler.SymbolManagement.Repository.Yaml.Callbacks;
using KSPCompiler.SymbolManagement.Repository.Yaml.Commands;
using KSPCompiler.SymbolManagement.Repository.Yaml.UITypes;
using KSPCompiler.SymbolManagement.Repository.Yaml.Variables;

using NUnit.Framework;

namespace KSPCompiler.Interactors.Tests.ApplicationServices;

[TestFixture]
public class SymbolLocalDatabaseApplicationServiceTest
{
    private static readonly string TestDataDirectory = Path.Combine(
        "ApplicationServices",
        "TestData",
        "SymbolLocalDatabaseApplicationServiceTest"
    );
    private static readonly string ImportTestDataDirectory = Path.Combine( TestDataDirectory, "import" );
    private static readonly string ExportTestDataDirectory = Path.Combine( TestDataDirectory, "export" );
    private static readonly string DeleteTestDataDirectory = Path.Combine( TestDataDirectory, "delete" );
    private static readonly string FindTestDataDirectory = Path.Combine( TestDataDirectory, "find" );

    [SetUp]
    public void Setup()
    {
        Directory.CreateDirectory( TestDataDirectory );
    }

    #region Variable
    [Test]
    public async Task ImportVariablesTest()
    {
        var importPath = new FilePath( Path.Combine( ImportTestDataDirectory, "variable.tsv" ) );
        var repositoryPath = new FilePath( Path.Combine( ImportTestDataDirectory, "repository_variable.yaml" ) );

        ITextContentReader reader = new LocalTextContentReader( importPath );
        ISymbolImporter<VariableSymbol> importer = new TsvVariableSymbolImporter( reader );
        using ISymbolRepository<VariableSymbol> repository = new VariableSymbolRepository( repositoryPath );
        var applicationService = new SymbolDatabaseApplicationService<VariableSymbol>( repository );

        var result = await applicationService.ImportAsync( importer );

        Assert.That( result.Success, Is.True );
    }

    [Test]
    public async Task ExportVariablesTest()
    {
        var exportPath = new FilePath( Path.Combine( ExportTestDataDirectory, "variable.tsv" ) );
        var repositoryPath = new FilePath( Path.Combine( ExportTestDataDirectory, "repository_variable.yaml" ) );

        ITextContentWriter writer = new LocalTextContentWriter( exportPath );
        ISymbolExporter<VariableSymbol> exporter = new TsvVariableSymbolExporter( writer );
        using ISymbolRepository<VariableSymbol> repository = new VariableSymbolRepository( repositoryPath );
        var applicationService = new SymbolDatabaseApplicationService<VariableSymbol>( repository );

        var result = await applicationService.ExportAsync( exporter, _ => true );

        Assert.That( result.Success, Is.True );
    }

    [Test]
    public async Task DeleteVariablesTest()
    {
        var repositoryPath = new FilePath( Path.Combine( DeleteTestDataDirectory, "repository_variable.yaml" ) );

        using ISymbolRepository<VariableSymbol> repository = new VariableSymbolRepository( repositoryPath );
        var applicationService = new SymbolDatabaseApplicationService<VariableSymbol>( repository );

        var result = await applicationService.DeleteAsync( _ => true );

        Assert.That( result.Success, Is.True );
        Assert.That( ( await repository.FindAllAsync() ).Any(), Is.False );
    }

    [Test]
    public async Task FindVariablesTest()
    {
        var repositoryPath = new FilePath( Path.Combine( FindTestDataDirectory, "repository_variable.yaml" ) );

        using ISymbolRepository<VariableSymbol> repository = new VariableSymbolRepository( repositoryPath );
        var applicationService = new SymbolDatabaseApplicationService<VariableSymbol>( repository );

        var result = await applicationService.FindAsync( _ => true );

        Assert.That( result.Success, Is.True );
        Assert.That( result.FoundSymbols, Is.Not.Empty );
    }

    [Test]
    public async Task ExportVariablesTemplateTest()
    {
        var exportPath = new FilePath( Path.Combine( ExportTestDataDirectory, "variable_template.tsv" ) );

        ITextContentWriter writer = new LocalTextContentWriter( exportPath );
        ISymbolExporter<VariableSymbol> exporter = new TsvVariableSymbolExporter( writer );
        var applicationService = new SymbolTemplateApplicationService<VariableSymbol>();
        var result = await applicationService.ExportAsync( exporter );

        Assert.That( result.Success, Is.True );
    }
    #endregion

    #region Command
    [Test]
    public async Task ImportCommandsTest()
    {
        var importPath = new FilePath( Path.Combine( ImportTestDataDirectory, "command.tsv" ) );
        var repositoryPath = new FilePath( Path.Combine( ImportTestDataDirectory, "repository_command.yaml" ) );

        ITextContentReader reader = new LocalTextContentReader( importPath );
        ISymbolImporter<CommandSymbol> importer = new TsvCommandSymbolImporter( reader );
        using ISymbolRepository<CommandSymbol> repository = new CommandSymbolRepository( repositoryPath );
        var applicationService = new SymbolDatabaseApplicationService<CommandSymbol>( repository );

        var result = await applicationService.ImportAsync( importer );

        Assert.That( result.Success, Is.True );
    }

    [Test]
    public async Task ExportCommandsTest()
    {
        var exportPath = new FilePath( Path.Combine( ExportTestDataDirectory, "command.tsv" ) );
        var repositoryPath = new FilePath( Path.Combine( ExportTestDataDirectory, "repository_command.yaml" ) );

        ITextContentWriter writer = new LocalTextContentWriter( exportPath );
        ISymbolExporter<CommandSymbol> exporter = new TsvCommandSymbolExporter( writer );
        using ISymbolRepository<CommandSymbol> repository = new CommandSymbolRepository( repositoryPath );
        var applicationService = new SymbolDatabaseApplicationService<CommandSymbol>( repository );

        var result = await applicationService.ExportAsync( exporter, _ => true );

        Assert.That( result.Success, Is.True );
    }

    [Test]
    public async Task DeleteCommandsTest()
    {
        var repositoryPath = new FilePath( Path.Combine( DeleteTestDataDirectory, "repository_command.yaml" ) );

        using ISymbolRepository<CommandSymbol> repository = new CommandSymbolRepository( repositoryPath );
        var applicationService = new SymbolDatabaseApplicationService<CommandSymbol>( repository );

        var result = await applicationService.DeleteAsync( _ => true );

        Assert.That( result.Success, Is.True );
        Assert.That( await repository.FindAllAsync(), Is.Empty );
    }

    [Test]
    public async Task FindCommandsTest()
    {
        var repositoryPath = new FilePath( Path.Combine( FindTestDataDirectory, "repository_command.yaml" ) );

        using ISymbolRepository<CommandSymbol> repository = new CommandSymbolRepository( repositoryPath );
        var applicationService = new SymbolDatabaseApplicationService<CommandSymbol>( repository );

        var result = await applicationService.FindAsync( _ => true );

        Assert.That( result.Success, Is.True );
        Assert.That( result.FoundSymbols, Is.Not.Empty );
    }

    [Test]
    public async Task ExportCommandsTemplateTest()
    {
        var exportPath = new FilePath( Path.Combine( ExportTestDataDirectory, "command_template.tsv" ) );

        ITextContentWriter writer = new LocalTextContentWriter( exportPath );
        ISymbolExporter<CommandSymbol> exporter = new TsvCommandSymbolExporter( writer );
        var applicationService = new SymbolTemplateApplicationService<CommandSymbol>();
        var result = await applicationService.ExportAsync( exporter );

        Assert.That( result.Success, Is.True );
    }
    #endregion

    #region Callback
    [Test]
    public async Task ImportCallbacksTest()
    {
        var importPath = new FilePath( Path.Combine( ImportTestDataDirectory, "callback.tsv" ) );
        var repositoryPath = new FilePath( Path.Combine( ImportTestDataDirectory, "repository_callback.yaml" ) );

        ITextContentReader reader = new LocalTextContentReader( importPath );
        ISymbolImporter<CallbackSymbol> importer = new TsvCallbackSymbolImporter( reader );
        using ISymbolRepository<CallbackSymbol> repository = new CallbackSymbolRepository( repositoryPath );
        var applicationService = new SymbolDatabaseApplicationService<CallbackSymbol>( repository );

        var result = await applicationService.ImportAsync( importer );

        Assert.That( result.Success, Is.True );
    }

    [Test]
    public async Task ExportCallbacksTest()
    {
        var exportPath = new FilePath( Path.Combine( ExportTestDataDirectory, "callback.tsv" ) );
        var repositoryPath = new FilePath( Path.Combine( ExportTestDataDirectory, "repository_callback.yaml" ) );

        ITextContentWriter writer = new LocalTextContentWriter( exportPath );
        ISymbolExporter<CallbackSymbol> exporter = new TsvCallbackSymbolExporter( writer );
        using ISymbolRepository<CallbackSymbol> repository = new CallbackSymbolRepository( repositoryPath );
        var applicationService = new SymbolDatabaseApplicationService<CallbackSymbol>( repository );

        var result = await applicationService.ExportAsync( exporter, _ => true );

        Assert.That( result.Success, Is.True );
    }

    [Test]
    public async Task DeleteCallbacksTest()
    {
        var repositoryPath = new FilePath( Path.Combine( DeleteTestDataDirectory, "repository_callback.yaml" ) );

        using ISymbolRepository<CallbackSymbol> repository = new CallbackSymbolRepository( repositoryPath );
        var applicationService = new SymbolDatabaseApplicationService<CallbackSymbol>( repository );

        var result = await applicationService.DeleteAsync( _ => true );

        Assert.That( result.Success, Is.True );
        Assert.That( await repository.FindAllAsync(), Is.Empty );
    }

    [Test]
    public async Task FindCallbacksTest()
    {
        var repositoryPath = new FilePath( Path.Combine( FindTestDataDirectory, "repository_callback.yaml" ) );

        using ISymbolRepository<CallbackSymbol> repository = new CallbackSymbolRepository( repositoryPath );
        var applicationService = new SymbolDatabaseApplicationService<CallbackSymbol>( repository );

        var result = await applicationService.FindAsync( _ => true );

        Assert.That( result.Success, Is.True );
        Assert.That( result.FoundSymbols, Is.Not.Empty );
    }

    [Test]
    public async Task ExportCallbacksTemplateTest()
    {
        var exportPath = new FilePath( Path.Combine( ExportTestDataDirectory, "callback_template.tsv" ) );

        ITextContentWriter writer = new LocalTextContentWriter( exportPath );
        ISymbolExporter<CallbackSymbol> exporter = new TsvCallbackSymbolExporter( writer );
        var applicationService = new SymbolTemplateApplicationService<CallbackSymbol>();
        var result = await applicationService.ExportAsync( exporter );

        Assert.That( result.Success, Is.True );
    }
    #endregion

    #region UIType
    [Test]
    public async Task ImportUITypeTest()
    {
        var importPath = new FilePath( Path.Combine( ImportTestDataDirectory, "ui_type.tsv" ) );
        var repositoryPath = new FilePath( Path.Combine( ImportTestDataDirectory, "repository_ui_type.yaml" ) );

        ITextContentReader reader = new LocalTextContentReader( importPath );
        ISymbolImporter<UITypeSymbol> importer = new TsvUITypeSymbolImporter( reader );
        using ISymbolRepository<UITypeSymbol> repository = new UITypeSymbolRepository( repositoryPath );
        var applicationService = new SymbolDatabaseApplicationService<UITypeSymbol>( repository );

        var result = await applicationService.ImportAsync( importer );

        Assert.That( result.Success, Is.True );
    }

    [Test]
    public async Task ExportUITypeTest()
    {
        var exportPath = new FilePath( Path.Combine( ExportTestDataDirectory, "ui_type.tsv" ) );
        var repositoryPath = new FilePath( Path.Combine( ExportTestDataDirectory, "repository_ui_type.yaml" ) );

        ITextContentWriter writer = new LocalTextContentWriter( exportPath );
        ISymbolExporter<UITypeSymbol> exporter = new TsvUITypeSymbolExporter( writer );
        using ISymbolRepository<UITypeSymbol> repository = new UITypeSymbolRepository( repositoryPath );
        var applicationService = new SymbolDatabaseApplicationService<UITypeSymbol>( repository );

        var result = await applicationService.ExportAsync( exporter, _ => true );

        Assert.That( result.Success, Is.True );
    }

    [Test]
    public async Task DeleteUITypeTest()
    {
        var repositoryPath = new FilePath( Path.Combine( DeleteTestDataDirectory, "repository_ui_type.yaml" ) );

        using ISymbolRepository<UITypeSymbol> repository = new UITypeSymbolRepository( repositoryPath );
        var applicationService = new SymbolDatabaseApplicationService<UITypeSymbol>( repository );

        var result = await applicationService.DeleteAsync( _ => true );

        Assert.That( result.Success, Is.True );
        Assert.That( await repository.FindAllAsync(), Is.Empty );
    }

    [Test]
    public async Task FindUITypeTest()
    {
        var repositoryPath = new FilePath( Path.Combine( FindTestDataDirectory, "repository_ui_type.yaml" ) );

        using ISymbolRepository<UITypeSymbol> repository = new UITypeSymbolRepository( repositoryPath );
        var applicationService = new SymbolDatabaseApplicationService<UITypeSymbol>( repository );

        var result = await applicationService.FindAsync( _ => true );

        Assert.That( result.Success, Is.True );
        Assert.That( result.FoundSymbols, Is.Not.Empty );
    }

    [Test]
    public async Task ExportUITypesTemplateTest()
    {
        var exportPath = new FilePath( Path.Combine( ExportTestDataDirectory, "ui_type_template.tsv" ) );

        ITextContentWriter writer = new LocalTextContentWriter( exportPath );
        ISymbolExporter<UITypeSymbol> exporter = new TsvUITypeSymbolExporter( writer );
        var applicationService = new SymbolTemplateApplicationService<UITypeSymbol>();
        var result = await applicationService.ExportAsync( exporter );

        Assert.That( result.Success, Is.True );
    }
    #endregion
}
