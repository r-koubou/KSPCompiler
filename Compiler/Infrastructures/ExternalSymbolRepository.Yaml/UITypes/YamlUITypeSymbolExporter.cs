using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Commons.Contents;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolRepository.Yaml.Commons;
using KSPCompiler.ExternalSymbolRepository.Yaml.UITypes.Translators;

namespace KSPCompiler.ExternalSymbolRepository.Yaml.UITypes;

public class YamlUITypeSymbolExporter : ISymbolExporter<UITypeSymbol>
{
    private readonly ITextContentWriter contentWriter;

    public YamlUITypeSymbolExporter( ITextContentWriter writer )
    {
        contentWriter = writer;
    }

    public async Task ExportAsync( IEnumerable<UITypeSymbol> symbols, CancellationToken cancellationToken = default )
    {
        var serializer = SerializerBuilderFactory.Create().Build();
        var root = new ToYamlTranslator().Translate( symbols );
        var yaml = serializer.Serialize( root );

        await contentWriter.WriteContentAsync( yaml, cancellationToken );
    }

    public void Dispose() {}
}
