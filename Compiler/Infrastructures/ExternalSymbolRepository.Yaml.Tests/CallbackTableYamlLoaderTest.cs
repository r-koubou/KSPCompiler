using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KSPCompiler.ExternalSymbolRepository.Yaml.Callbacks;
using KSPCompiler.ExternalSymbolRepository.Yaml.Callbacks.Model;
using KSPCompiler.ExternalSymbolRepository.Yaml.Commons;
using KSPCompiler.Infrastructures.Commons.LocalStorages;

using NUnit.Framework;

namespace KSPCompiler.ExternalSymbolRepository.Yaml.Tests;

[TestFixture]
public class CallbackTableYamlLoaderTest
{
    private static readonly string TestDataDirectory = Path.Combine( "TestData", "CallbackTableYamlLoaderTest" );

    [Test]
    public void LoadYamlTest()
    {
        var path = Path.Combine( TestDataDirectory, "CallbackTable.yaml" );
        var yaml = File.ReadAllText( path, Encoding.UTF8 );
        var deserializer = DeserializerBuilderFactory.Create().Build();
        var definition = deserializer.Deserialize<RootObject>( yaml );

        Assert.NotNull( definition );

        Assert.IsTrue( definition.Version == RootObject.CurrentVersion );
        Assert.IsTrue( definition.Symbols.Count > 0 );

        Assert.IsTrue( !string.IsNullOrEmpty( definition.Symbols[ 0 ].Name ) );
        Assert.IsTrue( definition.Symbols[ 0 ].Reserved );
        Assert.IsTrue( !string.IsNullOrEmpty( definition.Symbols[ 0 ].Description ) );


        // Arguments
        Assert.IsTrue( definition.Symbols[ 0 ].Arguments.Any() );

        foreach( var arg in definition.Symbols[ 0 ].Arguments )
        {
            Assert.IsTrue( !string.IsNullOrEmpty( arg.Name ) );
            Assert.IsTrue( arg.RequiredDeclare );
            Assert.IsTrue( !string.IsNullOrEmpty( arg.Description ) );
        }
    }

    [Test]
    public async Task TranslateLoadedSymbolTableAsyncTest()
    {
        var path = Path.Combine( TestDataDirectory, "CallbackTable.yaml" );
        var importer = new YamlCallbackSymbolImporter( new LocalTextContentReader( path ) );
        var symbols = await importer.ImportAsync();

        Assert.IsTrue( symbols.Any() );

        var symbol = symbols.First();
        Assert.IsTrue( !string.IsNullOrEmpty( symbol.Name.Value ) );
        Assert.IsTrue( symbol.Reserved );
        Assert.IsTrue( symbol.AllowMultipleDeclaration );
        Assert.IsTrue( !string.IsNullOrEmpty( symbol.Description.Value ) );

        // Arguments
        Assert.IsTrue( symbol.Arguments.Any() );

        foreach( var arg in symbol.Arguments )
        {
            Assert.IsTrue( !string.IsNullOrEmpty( arg.Name ) );
            Assert.IsTrue( arg.RequiredDeclareOnInit );
            Assert.IsTrue( !string.IsNullOrEmpty( arg.Description ) );
        }
    }
}
