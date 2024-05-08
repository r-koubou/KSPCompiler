using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Commons.Contents;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolRepository.Yaml.Commands.Model;
using KSPCompiler.ExternalSymbolRepository.Yaml.Commands.Translators;
using KSPCompiler.ExternalSymbolRepository.Yaml.Commons;
using KSPCompiler.UseCases.Symbols.Commons;

namespace KSPCompiler.ExternalSymbolRepository.Yaml.Commands;

public class YamlCommandSymbolImporter : IExternalCommandSymbolImporter
{
    private readonly ITextContentReader contentReader;

    public YamlCommandSymbolImporter( ITextContentReader reader )
    {
        contentReader = reader;
    }

    public async Task<ISymbolTable<CommandSymbol>> ImportAsync( CancellationToken cancellationToken = default )
    {
        var yaml = await contentReader.ReadContentAsync( cancellationToken );
        var deserializer = DeserializerBuilderFactory.Create().Build();
        var rootObject = deserializer.Deserialize<RootObject>(yaml);

        return new FromYamlTranslator().Translate( rootObject );
    }
}