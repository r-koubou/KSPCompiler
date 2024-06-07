using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Commons.Contents;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolRepository.Yaml.Callbacks.Model;
using KSPCompiler.ExternalSymbolRepository.Yaml.Callbacks.Translators;
using KSPCompiler.ExternalSymbolRepository.Yaml.Commons;

namespace KSPCompiler.ExternalSymbolRepository.Yaml.Callbacks;

public class YamlCallbackSymbolImporter : ISymbolImporter<CallbackSymbol>
{
    private readonly ITextContentReader contentReader;

    public YamlCallbackSymbolImporter( ITextContentReader reader )
    {
        contentReader = reader;
    }

    public async Task<IReadOnlyCollection<CallbackSymbol>> ImportAsync( CancellationToken cancellationToken = default )
    {
        var yaml = await contentReader.ReadContentAsync( cancellationToken );
        var deserializer = DeserializerBuilderFactory.Create().Build();
        var rootObject = deserializer.Deserialize<RootObject>( yaml );

        return new FromYamlTranslator().Translate( rootObject );
    }
}
