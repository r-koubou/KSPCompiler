using System.IO;
using System.Threading.Tasks;

using KSPCompiler.Features.SymbolManagement.Gateways;
using KSPCompiler.Features.SymbolManagement.UseCase.ApplicationServices;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Abstractions.Contents;
using KSPCompiler.Shared.IO.Abstractions.Symbol;
using KSPCompiler.Shared.IO.Local;
using KSPCompiler.Shared.IO.Symbols.Tsv.Callbacks;
using KSPCompiler.Shared.IO.Symbols.Tsv.Commands;
using KSPCompiler.Shared.IO.Symbols.Tsv.UITypes;
using KSPCompiler.Shared.IO.Symbols.Tsv.Variables;
using KSPCompiler.Shared.IO.Symbols.Yaml.Callbacks;
using KSPCompiler.Shared.IO.Symbols.Yaml.Commands;
using KSPCompiler.Shared.IO.Symbols.Yaml.UITypes;
using KSPCompiler.Shared.IO.Symbols.Yaml.Variables;
using KSPCompiler.Shared.Path;
using KSPCompiler.SymbolManagement.Repository.Yaml;

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

        ITextContentWriter contentWriter = new LocalTextContentWriter( repositoryPath );
        ISymbolExporter<VariableSymbol> repositoryWriter = new YamlVariableSymbolExporter( contentWriter );

        using ISymbolRepository<VariableSymbol> repository = new VariableSymbolRepository( repositoryWriter: repositoryWriter );
        var applicationService = new SymbolDatabaseApplicationService<VariableSymbol>( repository );

        ITextContentReader tsvReader = new LocalTextContentReader( importPath );
        ISymbolImporter<VariableSymbol> tsvImporter = new TsvVariableSymbolImporter( tsvReader );

        var result = await applicationService.ImportAsync( tsvImporter );

        Assert.That( result.Success, Is.True );
    }

    [Test]
    public async Task ExportVariablesTest()
    {
        var exportPath = new FilePath( Path.Combine( ExportTestDataDirectory, "variable.tsv" ) );
        var repositoryPath = new FilePath( Path.Combine( ExportTestDataDirectory, "repository_variable.yaml" ) );

        ITextContentReader contentReader = new LocalTextContentReader( repositoryPath );
        ISymbolImporter<VariableSymbol> repositoryReader = new YamlVariableSymbolImporter( contentReader );

        using ISymbolRepository<VariableSymbol> repository = new VariableSymbolRepository( repositoryReader: repositoryReader );
        var applicationService = new SymbolDatabaseApplicationService<VariableSymbol>( repository );

        ITextContentWriter writer = new LocalTextContentWriter( exportPath );
        ISymbolExporter<VariableSymbol> exporter = new TsvVariableSymbolExporter( writer );

        var result = await applicationService.ExportAsync( exporter, _ => true );

        Assert.That( result.Success, Is.True );
    }

    [Test]
    public async Task DeleteVariablesTest()
    {
        var repositoryPath = new FilePath( Path.Combine( DeleteTestDataDirectory, "repository_variable.yaml" ) );

        ITextContentReader contentReader = new LocalTextContentReader( repositoryPath );
        ISymbolImporter<VariableSymbol> repositoryReader = new YamlVariableSymbolImporter( contentReader );

        ITextContentWriter contentWriter = new LocalTextContentWriter( repositoryPath );
        ISymbolExporter<VariableSymbol> repositoryWriter = new YamlVariableSymbolExporter( contentWriter );

        using ISymbolRepository<VariableSymbol> repository = new VariableSymbolRepository( repositoryReader: repositoryReader, repositoryWriter: repositoryWriter );
        var applicationService = new SymbolDatabaseApplicationService<VariableSymbol>( repository );

        var result = await applicationService.DeleteAsync( _ => true );

        Assert.That( result.Success, Is.True );
        Assert.That( (await repository.ToListAsync()).Count != 0, Is.False );
    }

    [Test]
    public async Task FindVariablesTest()
    {
        var repositoryPath = new FilePath( Path.Combine( FindTestDataDirectory, "repository_variable.yaml" ) );

        ITextContentReader contentReader = new LocalTextContentReader( repositoryPath );
        ISymbolImporter<VariableSymbol> repositoryReader = new YamlVariableSymbolImporter( contentReader );

        using ISymbolRepository<VariableSymbol> repository = new VariableSymbolRepository( repositoryReader: repositoryReader );
        var applicationService = new SymbolDatabaseApplicationService<VariableSymbol>( repository );

        var result = await applicationService.FindAsync( _ => true );

        Assert.That( result.Success, Is.True );
        Assert.That( result.FoundSymbols, Is.Not.Empty );
    }

    [Test]
    public async Task ExportVariablesTemplateTest()
    {
        var applicationService = new SymbolTemplateApplicationService<VariableSymbol>();

        var exportPath = new FilePath( Path.Combine( ExportTestDataDirectory, "variable_template.tsv" ) );

        ITextContentWriter writer = new LocalTextContentWriter( exportPath );
        ISymbolExporter<VariableSymbol> exporter = new TsvVariableSymbolExporter( writer );

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

        ITextContentWriter contentWriter = new LocalTextContentWriter( repositoryPath );
        ISymbolExporter<CommandSymbol> repositoryWriter = new YamlCommandSymbolExporter( contentWriter );

        using ISymbolRepository<CommandSymbol> repository = new CommandSymbolRepository( repositoryWriter: repositoryWriter );

        var applicationService = new SymbolDatabaseApplicationService<CommandSymbol>( repository );

        ITextContentReader reader = new LocalTextContentReader( importPath );
        ISymbolImporter<CommandSymbol> importer = new TsvCommandSymbolImporter( reader );

        var result = await applicationService.ImportAsync( importer );

        Assert.That( result.Success, Is.True );
    }

    [Test]
    public async Task ExportCommandsTest()
    {
        var exportPath = new FilePath( Path.Combine( ExportTestDataDirectory, "command.tsv" ) );
        var repositoryPath = new FilePath( Path.Combine( ExportTestDataDirectory, "repository_command.yaml" ) );

        ITextContentReader contentReader = new LocalTextContentReader( repositoryPath );
        ISymbolImporter<CommandSymbol> repositoryReader = new YamlCommandSymbolImporter( contentReader );

        using ISymbolRepository<CommandSymbol> repository = new CommandSymbolRepository( repositoryReader: repositoryReader );
        var applicationService = new SymbolDatabaseApplicationService<CommandSymbol>( repository );

        ITextContentWriter writer = new LocalTextContentWriter( exportPath );
        ISymbolExporter<CommandSymbol> exporter = new TsvCommandSymbolExporter( writer );

        var result = await applicationService.ExportAsync( exporter, _ => true );

        Assert.That( result.Success, Is.True );
    }

    [Test]
    public async Task DeleteCommandsTest()
    {
        var repositoryPath = new FilePath( Path.Combine( DeleteTestDataDirectory, "repository_command.yaml" ) );

        ITextContentReader contentReader = new LocalTextContentReader( repositoryPath );
        ISymbolImporter<CommandSymbol> repositoryReader = new YamlCommandSymbolImporter( contentReader );

        ITextContentWriter contentWriter = new LocalTextContentWriter( repositoryPath );
        ISymbolExporter<CommandSymbol> repositoryWriter = new YamlCommandSymbolExporter( contentWriter );

        using ISymbolRepository<CommandSymbol> repository = new CommandSymbolRepository( repositoryReader: repositoryReader, repositoryWriter: repositoryWriter );
        var applicationService = new SymbolDatabaseApplicationService<CommandSymbol>( repository );

        var result = await applicationService.DeleteAsync( _ => true );

        Assert.That( result.Success, Is.True );
        Assert.That( (await repository.ToListAsync()).Count != 0, Is.False );
    }

    [Test]
    public async Task FindCommandsTest()
    {
        var repositoryPath = new FilePath( Path.Combine( FindTestDataDirectory, "repository_command.yaml" ) );

        ITextContentReader contentReader = new LocalTextContentReader( repositoryPath );
        ISymbolImporter<CommandSymbol> repositoryReader = new YamlCommandSymbolImporter( contentReader );

        using ISymbolRepository<CommandSymbol> repository = new CommandSymbolRepository( repositoryReader: repositoryReader );
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

        ITextContentWriter contentWriter = new LocalTextContentWriter( repositoryPath );
        ISymbolExporter<CallbackSymbol> repositoryWriter = new YamlCallbackSymbolExporter( contentWriter );

        using ISymbolRepository<CallbackSymbol> repository = new CallbackSymbolRepository( repositoryWriter: repositoryWriter );

        var applicationService = new SymbolDatabaseApplicationService<CallbackSymbol>( repository );

        ITextContentReader reader = new LocalTextContentReader( importPath );
        ISymbolImporter<CallbackSymbol> importer = new TsvCallbackSymbolImporter( reader );

        var result = await applicationService.ImportAsync( importer );

        Assert.That( result.Success, Is.True );
    }

    [Test]
    public async Task ExportCallbacksTest()
    {
        var exportPath = new FilePath( Path.Combine( ExportTestDataDirectory, "callback.tsv" ) );
        var repositoryPath = new FilePath( Path.Combine( ExportTestDataDirectory, "repository_callback.yaml" ) );

        ITextContentReader contentReader = new LocalTextContentReader( repositoryPath );
        ISymbolImporter<CallbackSymbol> repositoryReader = new YamlCallbackSymbolImporter( contentReader );

        using ISymbolRepository<CallbackSymbol> repository = new CallbackSymbolRepository( repositoryReader: repositoryReader );
        var applicationService = new SymbolDatabaseApplicationService<CallbackSymbol>( repository );

        ITextContentWriter writer = new LocalTextContentWriter( exportPath );
        ISymbolExporter<CallbackSymbol> exporter = new TsvCallbackSymbolExporter( writer );

        var result = await applicationService.ExportAsync( exporter, _ => true );

        Assert.That( result.Success, Is.True );
    }

    [Test]
    public async Task DeleteCallbacksTest()
    {
        var repositoryPath = new FilePath( Path.Combine( DeleteTestDataDirectory, "repository_callback.yaml" ) );

        ITextContentReader contentReader = new LocalTextContentReader( repositoryPath );
        ISymbolImporter<CallbackSymbol> repositoryReader = new YamlCallbackSymbolImporter( contentReader );

        ITextContentWriter contentWriter = new LocalTextContentWriter( repositoryPath );
        ISymbolExporter<CallbackSymbol> repositoryWriter = new YamlCallbackSymbolExporter( contentWriter );

        using ISymbolRepository<CallbackSymbol> repository = new CallbackSymbolRepository( repositoryReader: repositoryReader, repositoryWriter: repositoryWriter );
        var applicationService = new SymbolDatabaseApplicationService<CallbackSymbol>( repository );

        var result = await applicationService.DeleteAsync( _ => true );

        Assert.That( result.Success, Is.True );
        Assert.That( (await repository.ToListAsync()).Count != 0, Is.False );
    }

    [Test]
    public async Task FindCallbacksTest()
    {
        var repositoryPath = new FilePath( Path.Combine( FindTestDataDirectory, "repository_callback.yaml" ) );

        ITextContentReader contentReader = new LocalTextContentReader( repositoryPath );
        ISymbolImporter<CallbackSymbol> repositoryReader = new YamlCallbackSymbolImporter( contentReader );

        using ISymbolRepository<CallbackSymbol> repository = new CallbackSymbolRepository( repositoryReader: repositoryReader );
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

        ITextContentWriter contentWriter = new LocalTextContentWriter( repositoryPath );
        ISymbolExporter<UITypeSymbol> repositoryWriter = new YamlUITypeSymbolExporter( contentWriter );

        using ISymbolRepository<UITypeSymbol> repository = new UITypeSymbolRepository( repositoryWriter: repositoryWriter );

        var applicationService = new SymbolDatabaseApplicationService<UITypeSymbol>( repository );

        ITextContentReader reader = new LocalTextContentReader( importPath );
        ISymbolImporter<UITypeSymbol> importer = new TsvUITypeSymbolImporter( reader );

        var result = await applicationService.ImportAsync( importer );

        Assert.That( result.Success, Is.True );
    }

    [Test]
    public async Task ExportUITypeTest()
    {
        var exportPath = new FilePath( Path.Combine( ExportTestDataDirectory, "ui_type.tsv" ) );
        var repositoryPath = new FilePath( Path.Combine( ExportTestDataDirectory, "repository_ui_type.yaml" ) );

        ITextContentReader contentReader = new LocalTextContentReader( repositoryPath );
        ISymbolImporter<UITypeSymbol> repositoryReader = new YamlUITypeSymbolImporter( contentReader );

        using ISymbolRepository<UITypeSymbol> repository = new UITypeSymbolRepository( repositoryReader: repositoryReader );
        var applicationService = new SymbolDatabaseApplicationService<UITypeSymbol>( repository );

        ITextContentWriter writer = new LocalTextContentWriter( exportPath );
        ISymbolExporter<UITypeSymbol> exporter = new TsvUITypeSymbolExporter( writer );

        var result = await applicationService.ExportAsync( exporter, _ => true );

        Assert.That( result.Success, Is.True );
    }

    [Test]
    public async Task DeleteUITypeTest()
    {
        var repositoryPath = new FilePath( Path.Combine( DeleteTestDataDirectory, "repository_ui_type.yaml" ) );

        ITextContentReader contentReader = new LocalTextContentReader( repositoryPath );
        ISymbolImporter<UITypeSymbol> repositoryReader = new YamlUITypeSymbolImporter( contentReader );

        ITextContentWriter contentWriter = new LocalTextContentWriter( repositoryPath );
        ISymbolExporter<UITypeSymbol> repositoryWriter = new YamlUITypeSymbolExporter( contentWriter );

        using ISymbolRepository<UITypeSymbol> repository = new UITypeSymbolRepository( repositoryReader: repositoryReader, repositoryWriter: repositoryWriter );
        var applicationService = new SymbolDatabaseApplicationService<UITypeSymbol>( repository );

        var result = await applicationService.DeleteAsync( _ => true );

        Assert.That( result.Success, Is.True );
        Assert.That( (await repository.ToListAsync()).Count != 0, Is.False );
    }

    [Test]
    public async Task FindUITypeTest()
    {
        var repositoryPath = new FilePath( Path.Combine( FindTestDataDirectory, "repository_ui_type.yaml" ) );

        ITextContentReader contentReader = new LocalTextContentReader( repositoryPath );
        ISymbolImporter<UITypeSymbol> repositoryReader = new YamlUITypeSymbolImporter( contentReader );

        using ISymbolRepository<UITypeSymbol> repository = new UITypeSymbolRepository( repositoryReader: repositoryReader );
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
