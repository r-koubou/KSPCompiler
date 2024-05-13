using System;
using System.IO;

using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolControllers;
using KSPCompiler.ExternalSymbolRepository.Tsv.Variables;
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
    var path = Path.Combine( outputDirectory, databaseName + ".yaml" );
    var exporter = new YamlVariableSymbolExporter( new LocalTextContentWriter( path ) );
    var interactor = new CreateVariableSymbolUseCase( exporter );
    var controller = new CreateVariableSymbolController( interactor );
    controller.Create();
}

static void ConvertVariableImp<TSymbol>( ISymbolImporter<TSymbol> source, ISymbolExporter<TSymbol> destination ) where TSymbol : SymbolBase
{
    // Load
    var loadInteractor = new ImportSymbolInteractor<TSymbol>( source );
    var loadController = new ImportSymbolController<TSymbol>( loadInteractor );

    var loadResult = loadController.Import();

    if( !loadResult.Result )
    {
        throw new InvalidOperationException("Load failed", loadResult.Error);
    }

    // Store
    var storeInteractor = new ExportSymbolInteractor<TSymbol>( destination );
    var storeController = new ExportSymbolController<TSymbol>( storeInteractor );

    storeController.Export( loadResult.OutputData );
}

static void TsvToYaml(
    [Option( 0, "tsv file")] string input,
    [Option( 1, "yaml file")] string output )
{
    var sourceRepository = new TsvVariableSymbolImporter( new LocalTextContentReader( input ) );
    var destinationRepository = new YamlVariableSymbolExporter( new LocalTextContentWriter( output ) );

    ConvertVariableImp( sourceRepository, destinationRepository );
}

static void YamlToTsv(
    [Option( 0, "yaml file")] string input,
    [Option( 1, "tsv file")] string output )
{
    var sourceRepository = new YamlVariableSymbolImporter( new LocalTextContentReader( input ) );
    var destinationRepository = new TsvVariableSymbolExporter( new LocalTextContentWriter( output ) );

    ConvertVariableImp( sourceRepository, destinationRepository );
}

#endregion
