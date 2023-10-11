using KSPCompiler.ExternalSymbolRepository.Tsv.Variables;
using KSPCompiler.ExternalSymbolRepository.Yaml.Variables;
using KSPCompiler.Interactor.Symbols;

namespace KSPCompiler.Apps.VariableSymbolDataManager;

public class TsvToYaml : ConsoleAppBase
{
    public void t2y( [Option("t")] string tsvPath, [Option("y")] string yamlPath )
    {
        var interactor = new ExternalSymbolConvertInteractor();
        using var tsvRepository = new TsvVariableSymbolRepository( tsvPath );
        using var yamlRepository = new YamlVariableSymbolRepository( yamlPath );
        interactor.Convert( tsvRepository, yamlRepository );
    }
}
