using System.IO;

using KSPCompiler.Commons.Path;
using KSPCompiler.ExternalSymbolRepository.Tsv.Variables;
using KSPCompiler.ExternalSymbolRepository.Yaml.Variables;
using KSPCompiler.Interactor.Symbols;
using KSPCompiler.UseCases.Symbols;

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
        var source = new FilePath( Path.Combine( TestDataDirectory, "Tsv", "VariableTable.tsv" ) );
        var destination = new FilePath( Path.Combine( OutputDirectory, "Yaml", "Variables.yaml" ) );

        var controller = new TsvSymbolTableToYamlSymbolTableFileConvertController( new ExternalSymbolConvertInteractor() );

        controller.Convert( source, destination );
        Assert.That( File.Exists( destination.Path ), Is.True );
    }
}


public class TsvSymbolTableToYamlSymbolTableFileConvertController
{
    private readonly IExternalSymbolConvertUseCase useCase;

    public TsvSymbolTableToYamlSymbolTableFileConvertController( IExternalSymbolConvertUseCase useCase )
    {
        this.useCase = useCase;
    }

    public void Convert(FilePath source, FilePath destination)
    {
        using var sourceRepository = new TsvVariableSymbolRepository( source );
        using var destRepository = new YamlVariableSymbolRepository( destination );

        useCase.Convert( sourceRepository, destRepository );
    }
}
