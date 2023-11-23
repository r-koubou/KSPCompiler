using System.IO;

using KSPCompiler.ExternalSymbolRepository.Tsv.Variables;
using KSPCompiler.ExternalSymbolRepository.Yaml.Variables;
using KSPCompiler.Infrastructures.Commons.LocalStorages;
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
        var sourceRepository = new TsvVariableSymbolRepository( new LocalTextContentReader( source ) );
        var loadInteractor = new VariableSymbolLoadInteractor( sourceRepository );
        var loadController = new VariableSymbolTableLoadController( loadInteractor );

        var loadResult = loadController.Load();
        Assert.True( loadResult.Reeult );
        Assert.True( loadResult.Table.Count > 0 );
        Assert.Null( loadResult.Error );

        // Store
        var destinationRepository = new YamlVariableSymbolRepository( destination );
        var storeInteractor = new VariableSymbolStoreInteractor( destinationRepository );
        var storeController = new VariableSymbolTableStoreController( storeInteractor );

        storeController.Store( loadResult.Table );
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
        var loadController = new VariableSymbolTableLoadController( loadInteractor );

        var loadResult = loadController.Load();
        Assert.True( loadResult.Reeult );
        Assert.True( loadResult.Table.Count > 0 );
        Assert.Null( loadResult.Error );

        // Store
        var destinationRepository = new TsvVariableSymbolRepository( new LocalTextContentWriter( destination ) );
        var storeInteractor = new VariableSymbolStoreInteractor( destinationRepository );
        var storeController = new VariableSymbolTableStoreController( storeInteractor );

        storeController.Store( loadResult.Table );
        Assert.That( File.Exists( destination ), Is.True );
    }
}
