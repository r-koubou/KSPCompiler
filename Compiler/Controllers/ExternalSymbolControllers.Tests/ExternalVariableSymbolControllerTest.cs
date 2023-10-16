using System.IO;

using KSPCompiler.ExternalSymbolRepository.Tsv.Variables;
using KSPCompiler.ExternalSymbolRepository.Yaml.Variables;
using KSPCompiler.Interactor.Symbols;

using NUnit.Framework;

namespace KSPCompiler.ExternalSymbolControllers.Tests;

[TestFixture]
public class ExternalVariableSymbolControllerTest
{
    private static readonly string TestDataDirectory = Path.Combine( "TestData", "ExternalSymbolControllersTest" );
    private static readonly string OutputDirectory  = Path.Combine( ".Temp", "ExternalSymbolControllersTest" );

    [Test]
    public void TsvToYamlConvertTest()
    {
        var source = Path.Combine( TestDataDirectory,    "VariableTable.tsv" );
        var destination = Path.Combine( OutputDirectory, "VariableTable-converted.yaml" );

        // Load
        var sourceRepository = new TsvVariableSymbolRepository( source );
        var loadInteractor = new VariableSymbolLoadInteractor( sourceRepository );
        var loadController = new ExternalVariableSymbolTableLoadController( loadInteractor );

        var table = loadController.Load();

        // Store
        var destinationRepository = new YamlVariableSymbolRepository( destination );
        var storeInteractor = new VariableSymbolStoreInteractor( destinationRepository );
        var storeController = new ExternalVariableSymbolTableStoreController( storeInteractor );

        storeController.Store( table );
        Assert.That( File.Exists( destination ), Is.True );
    }

    [Test]
    public void YamlToTsvConvertTest()
    {
        var source = Path.Combine( TestDataDirectory,    "VariableTable.yaml" );
        var destination = Path.Combine( OutputDirectory, "VariableTable-converted.tsv" );

        // Load
        var sourceRepository = new YamlVariableSymbolRepository( source );
        var loadInteractor = new VariableSymbolLoadInteractor( sourceRepository );
        var loadController = new ExternalVariableSymbolTableLoadController( loadInteractor );

        var table = loadController.Load();

        // Store
        var destinationRepository = new TsvVariableSymbolRepository( destination );
        var storeInteractor = new VariableSymbolStoreInteractor( destinationRepository );
        var storeController = new ExternalVariableSymbolTableStoreController( storeInteractor );

        storeController.Store( table );
        Assert.That( File.Exists( destination ), Is.True );
    }
}
