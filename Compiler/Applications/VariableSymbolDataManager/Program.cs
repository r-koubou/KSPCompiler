using System;

using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolControllers;
using KSPCompiler.ExternalSymbolRepository.Yaml.Variables;
using KSPCompiler.Infrastructures.Commons.LocalStorages;
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

static void ConvertVariableImp( IVariableSymbolRepository source, IVariableSymbolRepository destination )
{
    // Load
    var loadInteractor = new VariableSymbolLoadInteractor( source );
    var loadController = new VariableSymbolTableLoadController( loadInteractor );

    var loadResult = loadController.Load();

    if( !loadResult.Reeult )
    {
        throw new InvalidOperationException("Load failed", loadResult.Error);
    }

    // Store
    var storeInteractor = new VariableSymbolStoreInteractor( destination );
    var storeController = new VariableSymbolTableStoreController( storeInteractor );

    storeController.Store( loadResult.Table );
}

static void TsvToYaml(
    [Option( 0, "tsv file")] string input,
    [Option( 1, "yaml file")] string output )
{
    var sourceRepository = new TsvVariableSymbolRepository( new LocalTextContentReader( input ) );
    var destinationRepository = new YamlVariableSymbolRepository( output );

    ConvertVariableImp( sourceRepository, destinationRepository );
}

static void YamlToTsv(
    [Option( 0, "yaml file")] string input,
    [Option( 1, "tsv file")] string output )
{
    var sourceRepository = new YamlVariableSymbolRepository( input );
    var destinationRepository = new TsvVariableSymbolRepository( new LocalTextContentWriter( output ) );

    ConvertVariableImp( sourceRepository, destinationRepository );
}
#endregion
