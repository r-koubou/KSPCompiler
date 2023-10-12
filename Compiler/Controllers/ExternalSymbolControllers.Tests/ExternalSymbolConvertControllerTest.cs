using System.IO;

using KSPCompiler.Commons.Path;
using KSPCompiler.Interactor.Symbols;

using NUnit.Framework;

namespace KSPCompiler.ExternalSymbolControllers.Tests;

[TestFixture]
public class ExternalSymbolConvertControllerTest
{
    private static readonly string TestDataDirectory = Path.Combine( "TestData", "ExternalSymbolControllersTest" );
    private static readonly string OutputDirectory  = Path.Combine( ".Temp", "ExternalSymbolControllersTest" );

    [Test]
    public void TsvToYamlConvertTest()
    {
        var source = new FilePath( Path.Combine( TestDataDirectory, "VariableTable.tsv" ) );
        var destination = new FilePath( Path.Combine( OutputDirectory, "Variables.yaml" ) );

        var controller = new VariableSymbolTableFileConvertController( new ExternalVariableSymbolConvertInteractor() );

        controller.TsvToYamlConvert( source, destination );
        Assert.That( File.Exists( destination.Path ), Is.True );
    }

    [Test]
    public void YamlToTsvConvertTest()
    {
        var source = new FilePath( Path.Combine( TestDataDirectory, "Variables.yaml" ) );
        var destination = new FilePath( Path.Combine( OutputDirectory, "VariableTable.tsv" ) );

        var controller = new VariableSymbolTableFileConvertController( new ExternalVariableSymbolConvertInteractor() );

        controller.TsvToYamlConvert( source, destination );
        Assert.That( File.Exists( destination.Path ), Is.True );
    }
}
