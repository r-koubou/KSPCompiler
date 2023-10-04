using System.IO;
using System.Text;

using KSPCompiler.Commons.Path;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolRepository.Yaml.Variables.Model.Variables;
using KSPCompiler.ExternalSymbolRepository.Yaml.Variables.Translators;
using KSPCompiler.Gateways;

using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace KSPCompiler.ExternalSymbolRepository.Yaml.Variables;

public class ExternalVariableSymbolRepository : IExternalSymbolRepository<VariableSymbol>
{
    private readonly FilePath yamlFilePath;

    public ExternalVariableSymbolRepository( FilePath yamlFilePath )
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

    public bool StoreSymbolTable( ISymbolTable<VariableSymbol> symbolTable )
    {
        return true;
    }
}
