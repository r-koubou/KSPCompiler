using System.IO;

using KSPCompiler.Commons.Path;
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
        var repository = new TsvExternalVariableSymbolRepository( new FilePath( path ) );
        var symbolTable = repository.LoadSymbolTable();

        Assert.IsTrue( symbolTable.Table.Count == 1 );
    }
}
