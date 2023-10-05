using System.IO;
using System.Text;

using KSPCompiler.Commons.Path;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolRepository.Yaml.Variables.Model;
using KSPCompiler.ExternalSymbolRepository.Yaml.Variables.Translators;
using KSPCompiler.Gateways;

using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace KSPCompiler.ExternalSymbolRepository.Yaml.Variables;

public class YamlExternalVariableSymbolRepository : IExternalSymbolRepository<VariableSymbol>
{
    private readonly FilePath yamlFilePath;

    public YamlExternalVariableSymbolRepository( FilePath yamlFilePath )
    {
        this.yamlFilePath = yamlFilePath;
    }

    public ISymbolTable<VariableSymbol> LoadSymbolTable()
    {
        var yaml = File.ReadAllText( yamlFilePath.Path, Encoding.UTF8 );
        var deserializer = new DeserializerBuilder()
                          .WithNamingConvention( CamelCaseNamingConvention.Instance )
                          .Build();
        var rootObject = deserializer.Deserialize<RootObject>(yaml);

        return new FromYamlTranslator().Translate( rootObject );
    }

    public void StoreSymbolTable( ISymbolTable<VariableSymbol> store )
    {
        var serializer = new SerializerBuilder()
                          .WithNamingConvention( CamelCaseNamingConvention.Instance )
                          .Build();
        var yaml = serializer.Serialize( store );
        File.WriteAllText( yamlFilePath.Path, yaml, Encoding.UTF8 );
    }
}
