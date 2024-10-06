using System.IO;

using KSPCompiler.Domain.Ast.Analyzers;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Parser.Antlr.Tests.Commons;

using NUnit.Framework;

namespace KSPCompiler.Parser.Antlr.Tests;

[TestFixture]
public class AntlrSemanticAnalyzeOperatorTest
{
    private static readonly string TestDataDirectory = Path.Combine( "TestData", "SemanticAnalyzeOperatorTest" );

    [Test]
    public void AddSameTypeTest()
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var ast = ParseTestUtility.Parse( TestDataDirectory, "add.txt" );
        var symbolAnalyzer = new SymbolCollector( compilerMessageManger );
        var semanticAnalyzer = new SemanticAnalyzer( compilerMessageManger, new VariableSymbolTable() );
        var abortTraverseToken = new AbortTraverseToken();

        Assert.DoesNotThrow( () => semanticAnalyzer.Analyze( ast, abortTraverseToken ) );
        compilerMessageManger.WriteTo( System.Console.Out );

        Assert.IsFalse( abortTraverseToken.Aborted );
        Assert.IsFalse( compilerMessageManger.Count( CompilerMessageLevel.Error ) > 0 );
    }

    [Test]
    public void AddInCompatibleTypesAreFailedTest()
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var ast = ParseTestUtility.Parse( TestDataDirectory, "add_incompatible.txt" );
        var symbolAnalyzer = new SymbolCollector( compilerMessageManger );
        var semanticAnalyzer = new SemanticAnalyzer( compilerMessageManger, new VariableSymbolTable() );
        var abortTraverseToken = new AbortTraverseToken();

        Assert.DoesNotThrow( () => semanticAnalyzer.Analyze( ast, abortTraverseToken ) );
        compilerMessageManger.WriteTo( System.Console.Out );

        Assert.IsTrue( abortTraverseToken.Aborted );
        Assert.IsTrue( compilerMessageManger.Count( CompilerMessageLevel.Error ) > 0 );
    }

}
