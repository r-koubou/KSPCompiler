using System.IO;
using System.Text;

using KSPCompiler.Commons.Path;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolRepository.Yaml.Variables;
using KSPCompiler.ExternalSymbolRepository.Yaml.Variables.Model;

using NUnit.Framework;

using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace KSPCompiler.ExternalSymbolRepository.Yaml.Tests;

[TestFixture]
public class VariableTableYamlLoaderTest
{
    private static readonly string TestDataDirectory = Path.Combine( "TestData", "VariableTableYamlLoaderTest" );

    [Test]
    public void LoadYamlTest()
    {
        var path = Path.Combine( TestDataDirectory, "VariableTable.yaml" );
        var yaml = File.ReadAllText( path, Encoding.UTF8 );
        var deserializer = new DeserializerBuilder()
                          .WithNamingConvention( CamelCaseNamingConvention.Instance )
                          .Build();
        var definition = deserializer.Deserialize<RootObject>( yaml );

        Assert.NotNull( definition );

        Assert.IsTrue( definition.Version != RootObject.UnknownVersion );
        Assert.IsTrue( definition.Symbols.Count > 0 );

        Assert.IsTrue( !string.IsNullOrEmpty( definition.Symbols[ 0 ].Name ) );
        Assert.IsTrue( definition.Symbols[ 0 ].Reserved );
        Assert.IsTrue( !string.IsNullOrEmpty( definition.Symbols[ 0 ].Description ) );
    }

    [Test]
    public void TryLoadTest()
    {
        var path = Path.Combine( TestDataDirectory, "invalid.yaml" );

        ISymbolRepository<VariableSymbol> repository = new YamlVariableSymbolRepository( new FilePath( path ) );
        var defaultTable = new VariableSymbolTable();
        var result = repository.TryLoad( () => defaultTable );

        Assert.AreSame( result, defaultTable );
    }

    [Test]
    public void TranslateLoadedSymbolTable()
    {
        var path = Path.Combine( TestDataDirectory, "VariableTable.yaml" );
        ISymbolRepository<VariableSymbol> repository = new YamlVariableSymbolRepository( new FilePath( path ) );
        var symbolTable = repository.Load();

        Assert.IsTrue( symbolTable.Table.Count > 0 );
    }
}
