using System.IO;
using System.Linq;

using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolRepository.Tsv.Variables;
using KSPCompiler.ExternalSymbolRepository.Yaml.Variables;
using KSPCompiler.Infrastructures.Commons.LocalStorages;
using KSPCompiler.Interactor.Symbols;
using KSPCompiler.UseCases.Symbols;

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
        var loadInteractor = new ImportSymbolInteractorOld<VariableSymbol>( importer );
        var loadController = new ImportSymbolControllerOld<VariableSymbol>( loadInteractor );

        var loadResult = loadController.Import();
        Assert.True( loadResult.Result );
        Assert.True( loadResult.OutputData.Any() );
        Assert.Null( loadResult.Error );

        // Store
        var exportInputPort = new ExportSymbolInputDataOld<VariableSymbol>( loadResult.OutputData );
        var exporter = new YamlVariableSymbolExporter( new LocalTextContentWriter( destination ) );
        var storeInteractor = new ExportSymbolInteractorOld<VariableSymbol>( exporter );
        var storeController = new ExportSymbolControllerOld<VariableSymbol>( storeInteractor );

        storeController.Export( exportInputPort );
        Assert.That( File.Exists( destination ), Is.True );
    }

    [Test]
    public void YamlToTsvConvertTest()
    {
        var source = Path.Combine( TestDataDirectory,    "VariableTable.yaml" );
        var destination = Path.Combine( OutputDirectory, "VariableTable-converted.tsv" );

        // Import
        var sourceRepository = new YamlVariableSymbolImporter( new LocalTextContentReader( source ) );
        var importInteractor = new ImportSymbolInteractorOld<VariableSymbol>( sourceRepository );
        var importController = new ImportSymbolControllerOld<VariableSymbol>( importInteractor );

        var importResult = importController.Import();
        Assert.True( importResult.Result );
        Assert.True( importResult.OutputData.Any() );
        Assert.Null( importResult.Error );

        // Export
        var exportParameter = new ExportSymbolInputDataOld<VariableSymbol>( importResult.OutputData );
        var destinationRepository = new TsvVariableSymbolExporter( new LocalTextContentWriter( destination ) );
        var exportInteractor = new ExportSymbolInteractorOld<VariableSymbol>( destinationRepository );
        var exportController = new ExportSymbolControllerOld<VariableSymbol>( exportInteractor );

        exportController.Export( exportParameter );
        Assert.That( File.Exists( destination ), Is.True );
    }
}
