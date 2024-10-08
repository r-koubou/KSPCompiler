using System.IO;

using KSPCompiler.Domain.Ast.Analyzers;
using KSPCompiler.Domain.Ast.Analyzers.Semantics;
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
    [TestCase( "add.txt" )]
    [TestCase( "sub.txt" )]
    [TestCase( "mul.txt" )]
    [TestCase( "div.txt" )]
    [TestCase( "mod.txt" )]
    [TestCase( "bit_or.txt" )]
    [TestCase( "bit_and.txt" )]
    public void AddSameTypeTest( string scriptPath )
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var ast = ParseTestUtility.Parse( TestDataDirectory, scriptPath );
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
