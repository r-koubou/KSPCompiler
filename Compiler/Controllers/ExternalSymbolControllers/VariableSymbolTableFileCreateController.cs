using System.IO;

using KSPCompiler.Commons.Path;
using KSPCompiler.ExternalSymbolRepository.Yaml.Variables;
using KSPCompiler.UseCases.Symbols;

namespace KSPCompiler.ExternalSymbolControllers;

public class VariableSymbolTableFileCreateController
{
    private readonly INewDatabaseCreateUseCase useCase;

    public VariableSymbolTableFileCreateController( INewDatabaseCreateUseCase useCase )
    {
        this.useCase = useCase;
    }

    public void Create( DirectoryPath outputDirectory, string databaseName )
    {
        var path = Path.Combine( outputDirectory.Path, databaseName + ".db.yaml" );
        using var repository = new YamlVariableSymbolRepository( path );

        useCase.Create( repository );
    }
}
