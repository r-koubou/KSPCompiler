using KSPCompiler.Commons.Path;
using KSPCompiler.ExternalSymbolRepository.Tsv.Variables;
using KSPCompiler.ExternalSymbolRepository.Yaml.Variables;
using KSPCompiler.UseCases.Symbols;

namespace KSPCompiler.ExternalSymbolControllers;

public class VariableSymbolTableFileConvertController
{
    private readonly IExternalVariableSymbolConvertUseCase useCase;

    public VariableSymbolTableFileConvertController( IExternalVariableSymbolConvertUseCase useCase )
    {
        this.useCase = useCase;
    }

    public void TsvToYamlConvert(FilePath source, FilePath destination)
    {
        using var sourceRepository = new TsvVariableSymbolRepository( source );
        using var destRepository = new YamlVariableSymbolRepository( destination );

        useCase.Convert( sourceRepository, destRepository );
    }

    public void YamlToTsvConvert(FilePath source, FilePath destination)
    {
        using var sourceRepository = new YamlVariableSymbolRepository( source );
        using var destRepository = new TsvVariableSymbolRepository( destination );

        useCase.Convert( sourceRepository, destRepository );
    }
}
