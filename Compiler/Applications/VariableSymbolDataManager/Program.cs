using KSPCompiler.Commons.Path;
using KSPCompiler.ExternalSymbolControllers;
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
    var inputPath = new FilePath( input );
    var outputPath = new FilePath( output );

    var interactor = new ExternalVariableSymbolConvertInteractor();
    var controller = new VariableSymbolTableFileConvertController( interactor );

    controller.TsvToYamlConvert( inputPath, outputPath );
}

static void YamlToTsv(
    [Option( 0, "yaml file")] string input,
    [Option( 1, "tsv file")] string output )
{
    var inputPath = new FilePath( input );
    var outputPath = new FilePath( output );

    var interactor = new ExternalVariableSymbolConvertInteractor();
    var controller = new VariableSymbolTableFileConvertController( interactor );

    controller.YamlToTsvConvert( inputPath, outputPath );
}
#endregion
