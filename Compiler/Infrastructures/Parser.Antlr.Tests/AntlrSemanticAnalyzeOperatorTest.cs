using System.IO;

using KSPCompiler.Domain.Ast.Analyzers;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Parser.Antlr.Tests.Commons;

using NUnit.Framework;

namespace KSPCompiler.Parser.Antlr.Tests;

[TestFixture]
public class AntlrSemanticAnalyzeOperatorTest
{
    private static readonly string TestDataDirectory = Path.Combine( "TestData", "SemanticAnalyzeOperatorTest" );

    [Test]
    public void AddTest()
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var ast = ParseTestUtility.Parse( TestDataDirectory, "add.txt" );
        var symbolAnalyzer = new SymbolCollector( compilerMessageManger );
        var semanticAnalyzer = new SemanticAnalyzer( compilerMessageManger );

        Assert.DoesNotThrow( () => semanticAnalyzer.Analyze( ast ) );
    }
}
