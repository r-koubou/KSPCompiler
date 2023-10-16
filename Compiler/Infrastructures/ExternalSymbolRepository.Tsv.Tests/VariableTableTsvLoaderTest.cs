using System.IO;

using KSPCompiler.Commons.Path;
using KSPCompiler.Domain.Tests;
using KSPCompiler.ExternalSymbolRepository.Tsv.Variables;

using NUnit.Framework;

namespace KSPCompiler.ExternalSymbolRepository.Tsv.Tests;

[TestFixture]
public class VariableTableTsvLoaderTest
{
    private static readonly string TestDataDirectory = Path.Combine( "TestData", "VariableTableTsvLoaderTest" );

    [Test]
    public void TranslateSymbolTable()
    {
        var path = Path.Combine( TestDataDirectory, "VariableTable.txt" );
        var repository = new TsvVariableSymbolRepository( new FilePath( path ) );
        var symbolTable = repository.LoadSymbolTable();

        Assert.IsTrue( symbolTable.Table.Count == 1 );
    }

    [Test]
    public void StoreTest()
    {
        var path = Path.Combine( TestDataDirectory, "VariableTable.tsv" );
        var repository = new TsvVariableSymbolRepository( new FilePath( path ) );
        var symbolTable = MockSymbolTableUtility.CreateDummy();

        repository.StoreSymbolTable( symbolTable );
    }
}
