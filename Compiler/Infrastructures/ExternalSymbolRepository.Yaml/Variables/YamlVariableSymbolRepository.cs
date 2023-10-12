using System.IO;
using System.Text;

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
        var root = new ToYamlTranslator().Translate( store );
        var yaml = serializer.Serialize( root );

        yamlFilePath.CreateDirectory();
        File.WriteAllText( yamlFilePath.Path, yaml, Encoding.UTF8 );
    }

    public void Dispose() {}
}
