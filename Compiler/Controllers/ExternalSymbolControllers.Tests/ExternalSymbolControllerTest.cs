using System.IO;
using System.Linq;

using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolRepository.Tsv.Variables;
using KSPCompiler.ExternalSymbolRepository.Yaml.Variables;
using KSPCompiler.Infrastructures.Commons.LocalStorages;
using KSPCompiler.Interactor.Symbols;

using NUnit.Framework;

namespace KSPCompiler.ExternalSymbolControllers.Tests;

[TestFixture]
public class ExternalSymbolControllerTest
{
    private static readonly string TestDataDirectory = Path.Combine( "TestData", "ExternalSymbolControllersTest" );
    private static readonly string OutputDirectory  = Path.Combine( ".Temp", "ExternalSymbolControllersTest" );

    [Test]
    public void TsvToYamlConvertTest()
    {
        var source = Path.Combine( TestDataDirectory,    "VariableTable.tsv" );
        var destination = Path.Combine( OutputDirectory, "VariableTable-converted.yaml" );

        // Load
        var importer = new TsvVariableSymbolImporter( new LocalTextContentReader( source ) );
        var loadInteractor = new ImportSymbolInteractor<VariableSymbol>( importer );
        var loadController = new ImportSymbolController<VariableSymbol>( loadInteractor );

        var loadResult = loadController.Import();
        Assert.True( loadResult.Result );
        Assert.True( loadResult.OutputData.Any() );
        Assert.Null( loadResult.Error );

        // Store
        var exporter = new YamlVariableSymbolExporter( new LocalTextContentWriter( destination ) );
        var storeInteractor = new ExportSymbolInteractor<VariableSymbol>( exporter );
        var storeController = new ExportSymbolController<VariableSymbol>( storeInteractor );

        storeController.Export( loadResult.OutputData );
        Assert.That( File.Exists( destination ), Is.True );
    }

    [Test]
    public void YamlToTsvConvertTest()
    {
        var source = Path.Combine( TestDataDirectory,    "VariableTable.yaml" );
        var destination = Path.Combine( OutputDirectory, "VariableTable-converted.tsv" );

        // Load
        var sourceRepository = new YamlVariableSymbolImporter( new LocalTextContentReader( source ) );
        var loadInteractor = new ImportSymbolInteractor<VariableSymbol>( sourceRepository );
        var loadController = new ImportSymbolController<VariableSymbol>( loadInteractor );

        var loadResult = loadController.Import();
        Assert.True( loadResult.Result );
        Assert.True( loadResult.OutputData.Any() );
        Assert.Null( loadResult.Error );

        // Store
        var destinationRepository = new TsvVariableSymbolExporter( new LocalTextContentWriter( destination ) );
        var storeInteractor = new ExportSymbolInteractor<VariableSymbol>( destinationRepository );
        var storeController = new ExportSymbolController<VariableSymbol>( storeInteractor );

        storeController.Export( loadResult.OutputData );
        Assert.That( File.Exists( destination ), Is.True );
    }
}
