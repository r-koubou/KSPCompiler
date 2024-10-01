using System.IO;
using System.Threading.Tasks;

using KSPCompiler.Commons.Contents;
using KSPCompiler.Commons.Path;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.Repositories;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Variables;
using KSPCompiler.ExternalSymbolRepository.Tsv.Variables;
using KSPCompiler.Infrastructures.Commons.LocalStorages;

using NUnit.Framework;

namespace KSPCompiler.SymbolDatabaseControllers.Tests;

[TestFixture]
public class SymbolLocalDatabaseControllerTest
{
    private static readonly string TestDataDirectory = Path.Combine( "TestData", "SymbolLocalDatabaseControllerTest" );
    private static readonly string ImportTestDataDirectory = Path.Combine( TestDataDirectory, "import" );

    [SetUp]
    public void Setup()
    {
        Directory.CreateDirectory( TestDataDirectory );
    }

    [Test]
    public async Task ImportVariablesTest()
    {
        var importPath = new FilePath( Path.Combine( ImportTestDataDirectory,     "variable.tsv" ) );
        var repositoryPath = new FilePath( Path.Combine( ImportTestDataDirectory, "repository_variable.json" ) );

        ITextContentReader reader = new LocalTextContentReader( importPath );
        ISymbolImporter<VariableSymbol> importer = new TsvVariableSymbolImporter( reader );
        using ISymbolRepository<VariableSymbol> repository = new VariableSymbolRepository( repositoryPath );
        var controller = new SymbolDatabaseController<VariableSymbol>( repository );

        var result = await controller.ImportAsync( importer );

        Assert.IsTrue( result.Success );
    }
}
