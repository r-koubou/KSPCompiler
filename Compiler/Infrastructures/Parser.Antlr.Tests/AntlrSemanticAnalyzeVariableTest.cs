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
public class AntlrSemanticAnalyzeVariableTest
{
    private static readonly string TestDataDirectory = Path.Combine( "TestData", "SemanticAnalyzeVariableTest" );

    [TestCase( "initializer_int.txt" )]
    public void VariableInitializerTest( string scriptPath )
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var ast = ParseTestUtility.Parse( TestDataDirectory, scriptPath );
        var symbolAnalyzer = new SymbolCollector( compilerMessageManger );
        var semanticAnalyzer = new SemanticAnalyzer( compilerMessageManger, symbolAnalyzer.Variables );
        var abortTraverseToken = new AbortTraverseToken();

        Assert.DoesNotThrow( () => semanticAnalyzer.Analyze( ast, abortTraverseToken ) );
        compilerMessageManger.WriteTo( System.Console.Out );

        Assert.IsFalse( abortTraverseToken.Aborted );
        Assert.IsFalse( compilerMessageManger.Count( CompilerMessageLevel.Error ) > 0 );
    }
}
