using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Commons.Contents;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolRepository.Yaml.Commons;
using KSPCompiler.ExternalSymbolRepository.Yaml.Variables.Translators;
using KSPCompiler.UseCases.Symbols.Commons;

namespace KSPCompiler.ExternalSymbolRepository.Yaml.Variables;

public class YamlVariableSymbolExporter : IExternalVariableSymbolExporter
{
    private readonly ITextContentWriter contentWriter;

    public YamlVariableSymbolExporter( ITextContentWriter writer )
    {
        contentWriter = writer;
    }

    public async Task ExportAsync( ISymbolTable<VariableSymbol> store, CancellationToken cancellationToken = default )
    {
        var serializer = SerializerBuilderFactory.Create().Build();
        var root = new ToYamlTranslator().Translate( store );
        var yaml = serializer.Serialize( root );

        await contentWriter.WriteContentAsync( yaml, cancellationToken );
    }

    public void Dispose() {}
}
