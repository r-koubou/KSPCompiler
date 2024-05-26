using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Commons.Contents;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolRepository.Yaml.Commons;
using KSPCompiler.ExternalSymbolRepository.Yaml.UITypes.Model;
using KSPCompiler.ExternalSymbolRepository.Yaml.UITypes.Translators;

namespace KSPCompiler.ExternalSymbolRepository.Yaml.UITypes;

public class YamlUITypeSymbolImporter : ISymbolImporter<UITypeSymbol>
{
    private readonly ITextContentReader contentReader;

    public YamlUITypeSymbolImporter( ITextContentReader reader )
    {
        contentReader = reader;
    }

    public async Task<IReadOnlyCollection<UITypeSymbol>> ImportAsync( CancellationToken cancellationToken = default )
    {
        var yaml = await contentReader.ReadContentAsync( cancellationToken );
        var deserializer = DeserializerBuilderFactory.Create().Build();
        var rootObject = deserializer.Deserialize<RootObject>( yaml );

        return new FromYamlTranslator().Translate( rootObject );
    }
}
