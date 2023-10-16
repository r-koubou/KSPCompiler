using KSPCompiler.Commons.Path;
using KSPCompiler.ExternalSymbolControllers;
using KSPCompiler.ExternalSymbolRepository.Tsv.Variables;
using KSPCompiler.ExternalSymbolRepository.Yaml.Variables;
using KSPCompiler.Interactor.Symbols;

var app = ConsoleApp.Create( args );
app.AddCommand( "newdb",    NewDb );
app.AddCommand( "tsv2yaml", TsvToYaml );
app.AddCommand( "yaml2tsv", YamlToTsv );

app.Run();

return;

#region Command Implementation

static void NewDb(
    [Option("d", "Output directory for database file")] string outputDirectory,
    [Option("f", "New database name")] string databaseName)
{
    var interactor = new NewDatabaseCreateInteractor();
    var controller = new VariableSymbolTableFileCreateController( interactor );
    controller.Create( outputDirectory, databaseName );
}

static void TsvToYaml(
    [Option( 0, "tsv file")] string input,
    [Option( 1, "yaml file")] string output )
{
    // Load
    var sourceRepository = new TsvVariableSymbolRepository( input );
    var loadInteractor = new VariableSymbolLoadInteractor( sourceRepository );
    var loadController = new ExternalVariableSymbolTableLoadController( loadInteractor );

    var table = loadController.Load();

    // Store
    var destinationRepository = new YamlVariableSymbolRepository( output );
    var storeInteractor = new VariableSymbolStoreInteractor( destinationRepository );
    var storeController = new ExternalVariableSymbolTableStoreController( storeInteractor );

    storeController.Store( table );
}

static void YamlToTsv(
    [Option( 0, "yaml file")] string input,
    [Option( 1, "tsv file")] string output )
{
    // Load
    var sourceRepository = new YamlVariableSymbolRepository( input );
    var loadInteractor = new VariableSymbolLoadInteractor( sourceRepository );
    var loadController = new ExternalVariableSymbolTableLoadController( loadInteractor );

    var table = loadController.Load();

    // Store
    var destinationRepository = new TsvVariableSymbolRepository( output );
    var storeInteractor = new VariableSymbolStoreInteractor( destinationRepository );
    var storeController = new ExternalVariableSymbolTableStoreController( storeInteractor );

    storeController.Store( table );
}
#endregion
