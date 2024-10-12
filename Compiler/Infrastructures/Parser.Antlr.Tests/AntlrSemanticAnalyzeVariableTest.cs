using System;
using System.IO;

using KSPCompiler.Domain.Ast.Analyzers;
using KSPCompiler.Domain.Ast.Analyzers.Semantics;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Parser.Antlr.Tests.Commons;

using NUnit.Framework;

namespace KSPCompiler.Parser.Antlr.Tests;

[TestFixture]
public class AntlrSemanticAnalyzeVariableTest
{
    private static readonly string TestDataDirectory = Path.Combine( "TestData", "SemanticAnalyzeVariableTest" );

    [TestCase( "assign_int.txt" )]
    public void AssignTest( string scriptPath )
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var ast = ParseTestUtility.Parse( TestDataDirectory, scriptPath );
        var symbolAnalyzer = new SymbolCollector( compilerMessageManger );
        var semanticAnalyzer = new SemanticAnalyzer( compilerMessageManger, symbolAnalyzer.Variables );
        var abortTraverseToken = new AbortTraverseToken();

        Assert.DoesNotThrow( () => semanticAnalyzer.Analyze( ast, abortTraverseToken ) );
        compilerMessageManger.WriteTo( Console.Out );

        Assert.IsFalse( abortTraverseToken.Aborted );
        Assert.IsFalse( compilerMessageManger.Count( CompilerMessageLevel.Error ) > 0 );
    }

    [TestCase( "assign_int_incompatible.txt" )]
    public void AssignIncompatibleTest( string scriptPath )
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var ast = ParseTestUtility.Parse( TestDataDirectory, scriptPath );
        var symbolAnalyzer = new SymbolCollector( compilerMessageManger );
        var semanticAnalyzer = new SemanticAnalyzer( compilerMessageManger, symbolAnalyzer.Variables );
        var abortTraverseToken = new AbortTraverseToken();

        Assert.DoesNotThrow( () => semanticAnalyzer.Analyze( ast, abortTraverseToken ) );
        compilerMessageManger.WriteTo( Console.Out );

        Assert.IsTrue( abortTraverseToken.Aborted );
        Assert.IsTrue( compilerMessageManger.Count( CompilerMessageLevel.Error ) > 0 );
    }
}
