using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Commons.Path;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolRepository.Yaml.Variables.Model;
using KSPCompiler.ExternalSymbolRepository.Yaml.Variables.Translators;

using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace KSPCompiler.ExternalSymbolRepository.Yaml.Variables;

public class YamlVariableSymbolRepository : IVariableSymbolRepository
{
    private readonly FilePath yamlFilePath;

    public YamlVariableSymbolRepository( FilePath yamlFilePath )
    {
        this.yamlFilePath = yamlFilePath;
    }

    public async Task<ISymbolTable<VariableSymbol>> LoadAsync( CancellationToken cancellationToken = default )
    {
        var yaml = await File.ReadAllTextAsync( yamlFilePath.Path, Encoding.UTF8, cancellationToken );
        var deserializer = new DeserializerBuilder()
                          .WithNamingConvention( CamelCaseNamingConvention.Instance )
                          .Build();
        var rootObject = deserializer.Deserialize<RootObject>(yaml);

        return new FromYamlTranslator().Translate( rootObject );
    }

    public async Task StoreAsync( ISymbolTable<VariableSymbol> store, CancellationToken cancellationToken = default )
    {
        var serializer = new SerializerBuilder()
                          .WithNamingConvention( CamelCaseNamingConvention.Instance )
                          .Build();
        var root = new ToYamlTranslator().Translate( store );
        var yaml = serializer.Serialize( root );

        yamlFilePath.CreateDirectory();
        await File.WriteAllTextAsync( yamlFilePath.Path, yaml, Encoding.UTF8, cancellationToken );
    }

    public void Dispose() {}
}
