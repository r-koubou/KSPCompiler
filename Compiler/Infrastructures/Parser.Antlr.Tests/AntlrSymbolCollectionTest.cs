using System.IO;

using KSPCompiler.Domain.Ast.Analyzers;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Parser.Antlr.Tests.Commons;

using NUnit.Framework;

namespace KSPCompiler.Parser.Antlr.Tests;

[TestFixture]
public class AntlrSymbolCollectionTest
{
    private static readonly string TestDataDirectory = Path.Combine( "TestData", "SymbolAnalyzeTest" );

    [Test]
    public void VariableSymbolAnalyzeTest()
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var ast = ParseTestUtility.Parse( TestDataDirectory, "VariableSymbolTest.txt" );
        var symbolAnalyzer = new SymbolCollector( compilerMessageManger );

        Assert.DoesNotThrow( () => { symbolAnalyzer.Analyze( ast ); } );
        compilerMessageManger.WriteTo( System.Console.Out );
        Assert.IsTrue( symbolAnalyzer.Variables.Count == 1 );
    }

    [Test]
    public void CallbackSymbolAnalyzeTest()
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var ast = ParseTestUtility.Parse( TestDataDirectory, "CallbackSymbolTest.txt" );
        var symbolAnalyzer = new SymbolCollector( compilerMessageManger );

        Assert.DoesNotThrow( () => { symbolAnalyzer.Analyze( ast ); } );
        compilerMessageManger.WriteTo( System.Console.Out );
        Assert.IsTrue( symbolAnalyzer.Callbacks.Count == 1 );
    }

    [Test]
    public void UserFunctionSymbolAnalyzeTest()
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var ast = ParseTestUtility.Parse( TestDataDirectory, "UserFunctionSymbolTest.txt" );
        var symbolAnalyzer = new SymbolCollector( compilerMessageManger );

        Assert.DoesNotThrow( () => { symbolAnalyzer.Analyze( ast ); } );
        compilerMessageManger.WriteTo( System.Console.Out );

        Assert.IsTrue( compilerMessageManger.Count() == 0 );
        Assert.IsTrue( symbolAnalyzer.UserFunctions.Count == 1 );
    }
}
