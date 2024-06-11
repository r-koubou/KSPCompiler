using System.IO;

using KSPCompiler.Domain.Ast.Analyzers;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Parser.Antlr.Tests.Commons;

using NUnit.Framework;

namespace KSPCompiler.Parser.Antlr.Tests;

[TestFixture]
public class AntlrSymbolAnalyzeTest
{
    private static readonly string TestDataDirectory = Path.Combine( "TestData", "SymbolAnalyzeTest" );

    [Test]
    public void VariableSymbolAnalyzeTest()
    {
        var ast = ParseTestUtility.Parse( TestDataDirectory, "VariableSymbolTest.txt" );
        var symbolAnalyzer = new SymbolAnalyzer( ICompilerMessageManger.CreateDefault() );

        Assert.DoesNotThrow( () => { symbolAnalyzer.Analyze( ast ); } );
        Assert.IsTrue( symbolAnalyzer.Variables.Count == 3 );
    }
}
