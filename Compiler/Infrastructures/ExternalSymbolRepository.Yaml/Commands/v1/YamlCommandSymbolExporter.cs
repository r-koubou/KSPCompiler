using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Commons.Contents;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolRepository.Yaml.Commands.v1.Translators;
using KSPCompiler.UseCases.Symbols.Commons;

using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace KSPCompiler.ExternalSymbolRepository.Yaml.Commands.v1;

public class YamlCommandSymbolExporter : IExternalCommandSymbolExporter
{
    private readonly ITextContentWriter contentWriter;

    public YamlCommandSymbolExporter( ITextContentWriter writer )
    {
        contentWriter = writer;
    }

    public async Task ExportAsync( ISymbolTable<CommandSymbol> store, CancellationToken cancellationToken = default )
    {
        var serializer = new SerializerBuilder()
                        .WithNamingConvention( CamelCaseNamingConvention.Instance )
                        .Build();
        var root = new ToYamlTranslator().Translate( store );
        var yaml = serializer.Serialize( root );

        await contentWriter.WriteContentAsync( yaml, cancellationToken );
    }

    public void Dispose() {}
}
