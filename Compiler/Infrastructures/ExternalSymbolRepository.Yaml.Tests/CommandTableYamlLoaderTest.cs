using System.IO;
using System.Text;
using System.Threading.Tasks;

using KSPCompiler.ExternalSymbol.Commons;
using KSPCompiler.ExternalSymbolRepository.Yaml.Commands;
using KSPCompiler.ExternalSymbolRepository.Yaml.Commands.Model.v1;
using KSPCompiler.Infrastructures.Commons.LocalStorages;

using NUnit.Framework;

using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace KSPCompiler.ExternalSymbolRepository.Yaml.Tests;

[TestFixture]
public class CommandTableYamlLoaderTest
{
    private static readonly string TestDataDirectory = Path.Combine( "TestData", "CommandTableYamlLoaderTest" );

    [Test]
    public void LoadYamlTest()
    {
        var path = Path.Combine( TestDataDirectory, "CommandTable.yaml" );
        var yaml = File.ReadAllText( path, Encoding.UTF8 );
        var deserializer = new DeserializerBuilder()
                          .WithNamingConvention( LowerCaseNamingConvention.Instance )
                          .Build();
        var definition = deserializer.Deserialize<RootObject>( yaml );

        Assert.NotNull( definition );

        Assert.IsTrue( definition.Version == RootObject.CurrentVersion );
        Assert.IsTrue( definition.Symbols.Count > 0 );

        Assert.IsTrue( !string.IsNullOrEmpty( definition.Symbols[ 0 ].Name ) );
        Assert.IsTrue( definition.Symbols[ 0 ].Reserved );
        Assert.IsTrue( !string.IsNullOrEmpty( definition.Symbols[ 0 ].Description ) );
    }

    [Test]
    public async Task TranslateLoadedSymbolTableAsyncTest()
    {
        var path = Path.Combine( TestDataDirectory, "CommandTable.yaml" );
        var importer = new YamlCommandSymbolImporter( new LocalTextContentReader( path ) );
        var symbolTable = await importer.ImportAsync();

        Assert.IsTrue( symbolTable.Table.Count > 0 );
    }

    [Test]
    public void CannotImportDuplicateSymbolTest()
    {
        var path = Path.Combine( TestDataDirectory, "DuplicateCommandTable.yaml" );
        var importer = new YamlCommandSymbolImporter( new LocalTextContentReader( path ) );

        Assert.ThrowsAsync<DuplicatedSymbolException>( async () => await importer.ImportAsync() );
    }
}
