using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Commons.Contents;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolRepository.Yaml.Commons;
using KSPCompiler.ExternalSymbolRepository.Yaml.Variables.Model;
using KSPCompiler.ExternalSymbolRepository.Yaml.Variables.Translators;
using KSPCompiler.UseCases.Symbols.Commons;

namespace KSPCompiler.ExternalSymbolRepository.Yaml.Variables;

public class YamlVariableSymbolImporter : IExternalVariableSymbolImporter
{
    private readonly ITextContentReader contentReader;

    public YamlVariableSymbolImporter( ITextContentReader reader )
    {
        contentReader = reader;
    }

    public async Task<ISymbolTable<VariableSymbol>> ImportAsync( CancellationToken cancellationToken = default )
    {
        var yaml = await contentReader.ReadContentAsync( cancellationToken );
        var deserializer = DeserializerBuilderFactory.Create().Build();
        var rootObject = deserializer.Deserialize<RootObject>(yaml);

        return new FromYamlTranslator().Translate( rootObject );
    }
}