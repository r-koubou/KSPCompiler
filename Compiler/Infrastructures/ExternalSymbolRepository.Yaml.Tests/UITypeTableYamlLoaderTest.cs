using System.IO;
using System.Text;
using System.Threading.Tasks;

using KSPCompiler.ExternalSymbolRepository.Yaml.Commons;
using KSPCompiler.ExternalSymbolRepository.Yaml.UITypes;
using KSPCompiler.ExternalSymbolRepository.Yaml.UITypes.Model;
using KSPCompiler.Infrastructures.Commons.LocalStorages;

using NUnit.Framework;

namespace KSPCompiler.ExternalSymbolRepository.Yaml.Tests;

[TestFixture]
public class UITypeTableYamlLoaderTest
{
    private static readonly string TestDataDirectory = Path.Combine( "TestData", "UITypeTableYamlLoaderTest" );

    [Test]
    public void LoadYamlTest()
    {
        var path = Path.Combine( TestDataDirectory, "UITypeTable.yaml" );
        var yaml = File.ReadAllText( path, Encoding.UTF8 );
        var deserializer = DeserializerBuilderFactory.Create().Build();
        var definition = deserializer.Deserialize<RootObject>( yaml );

        Assert.NotNull( definition );

        Assert.IsTrue( definition.Version == RootObject.CurrentVersion );
        Assert.IsTrue( definition.Symbols.Count > 0 );

        Assert.IsTrue( !string.IsNullOrEmpty( definition.Symbols[ 0 ].Name ) );
        Assert.IsTrue( !string.IsNullOrEmpty( definition.Symbols[ 0 ].Description ) );
    }

    [Test]
    public async Task TranslateLoadedSymbolTableAsyncTest()
    {
        var path = Path.Combine( TestDataDirectory, "UITypeTable.yaml" );
        var importer = new YamlUITypeSymbolImporter( new LocalTextContentReader( path ) );
        var symbols = await importer.ImportAsync();

        Assert.IsTrue( symbols.Count > 0 );
    }
}
