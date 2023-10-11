using KSPCompiler.Commons.Path;
using KSPCompiler.ExternalSymbolRepository.Tsv.Variables;
using KSPCompiler.ExternalSymbolRepository.Yaml.Variables;

var app = ConsoleApp.Create( args );
app.AddCommand( "tsv2yaml", TsvToYaml );

app.Run();

return;

static void TsvToYaml(
    [Option( 0, "tsv file")] string input,
    [Option( 1, "yaml file")] string output )
{
    var inputPath = new FilePath( input );
    var outputPath = new FilePath( output );

    using var tsvRepository = new TsvVariableSymbolRepository( inputPath );
    using var yamlRepository = new YamlVariableSymbolRepository( outputPath );

    if( outputPath.Exists )
    {
        var sourceTable = tsvRepository.LoadSymbolTable();
        var targetTable = yamlRepository.LoadSymbolTable();

        targetTable.Merge( sourceTable );
        yamlRepository.StoreSymbolTable( targetTable );
    }
    else
    {
        var sourceTable = tsvRepository.LoadSymbolTable();
        yamlRepository.StoreSymbolTable( sourceTable );
    }
}
