using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Commons.Contents;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolRepository.Yaml.Variables.Model;
using KSPCompiler.ExternalSymbolRepository.Yaml.Variables.Translators;
using KSPCompiler.UseCases.Symbols.Commons;

using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

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
        var deserializer = new DeserializerBuilder()
                          .WithNamingConvention( LowerCaseNamingConvention.Instance )
                          .Build();
        var rootObject = deserializer.Deserialize<RootObject>(yaml);

        return new FromYamlTranslator().Translate( rootObject );
    }
}
