using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Commons.Contents;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolRepository.Yaml.Callbacks.Translators;
using KSPCompiler.ExternalSymbolRepository.Yaml.Commons;

namespace KSPCompiler.ExternalSymbolRepository.Yaml.Callbacks;

public class YamlCallbackSymbolExporter : ISymbolExporter<CallbackSymbol>
{
    private readonly ITextContentWriter contentWriter;

    public YamlCallbackSymbolExporter( ITextContentWriter writer )
    {
        contentWriter = writer;
    }

    public async Task ExportAsync( IEnumerable<CallbackSymbol> symbols, CancellationToken cancellationToken = default )
    {
        var serializer = SerializerBuilderFactory.Create().Build();
        var root = new ToYamlTranslator().Translate( symbols );
        var yaml = serializer.Serialize( root );

        await contentWriter.WriteContentAsync( yaml, cancellationToken );
    }

    public void Dispose() {}
}
