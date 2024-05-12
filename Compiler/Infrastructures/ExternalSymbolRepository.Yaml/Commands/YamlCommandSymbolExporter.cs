using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Commons.Contents;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolRepository.Yaml.Commands.Translators;
using KSPCompiler.ExternalSymbolRepository.Yaml.Commons;
using KSPCompiler.UseCases.Symbols.Commons;

namespace KSPCompiler.ExternalSymbolRepository.Yaml.Commands;

public class YamlCommandSymbolExporter : IExternalCommandSymbolExporter
{
    private readonly ITextContentWriter contentWriter;

    public YamlCommandSymbolExporter( ITextContentWriter writer )
    {
        contentWriter = writer;
    }

    public async Task ExportAsync( ISymbolTable<CommandSymbol> store, CancellationToken cancellationToken = default )
    {
        var serializer = SerializerBuilderFactory.Create().Build();
        var root = new ToYamlTranslator().Translate( store );
        var yaml = serializer.Serialize( root );

        await contentWriter.WriteContentAsync( yaml, cancellationToken );
    }

    public void Dispose() {}
}
