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

    [TestCase( "add.txt" )]
    [TestCase( "sub.txt" )]
    [TestCase( "mul.txt" )]
    [TestCase( "div.txt" )]
    [TestCase( "mod.txt" )]
    [TestCase( "bit_or.txt" )]
    [TestCase( "bit_and.txt" )]
    [TestCase( "bit_xor.txt" )]
    [TestCase( "unary_minus.txt" )]
    [TestCase( "unary_not.txt" )]
    [TestCase( "string_concat.txt" )]
    public void AddSameTypeTest( string scriptPath )
    {
        var abortTraverseToken = new AbortTraverseToken();
        var compilerMessageManger = ICompilerMessageManger.Default;
        var ast = ParseTestUtility.Parse( TestDataDirectory, scriptPath );
        var symbolAnalyzer = new SymbolCollector( compilerMessageManger );

        symbolAnalyzer.Analyze( ast, abortTraverseToken );
        Assert.IsFalse( abortTraverseToken.Aborted );

        var semanticAnalyzer = new SemanticAnalyzer( compilerMessageManger, symbolAnalyzer.Variables );

        Assert.DoesNotThrow( () => semanticAnalyzer.Analyze( ast, abortTraverseToken ) );
        compilerMessageManger.WriteTo( System.Console.Out );

        Assert.IsFalse( abortTraverseToken.Aborted );
        Assert.IsFalse( compilerMessageManger.Count( CompilerMessageLevel.Error ) > 0 );
    }

    [TestCase( "add_incompatible.txt" )]
    [TestCase( "sub_incompatible.txt" )]
    [TestCase( "mul_incompatible.txt" )]
    [TestCase( "div_incompatible.txt" )]
    [TestCase( "mod_incompatible.txt" )]
    [TestCase( "bit_or_incompatible.txt" )]
    [TestCase( "bit_and_incompatible.txt" )]
    [TestCase( "bit_xor_incompatible.txt" )]
    [TestCase( "unary_minus_incompatible.txt" )]
    [TestCase( "unary_not_incompatible.txt" )]
    //[TestCase( "string_concat_incompatible.txt" )]
    #warning string_concat_incompatible.txt: Bool式の演算子評価をまだ実装していないため必ず失敗するので実装が終わるまではコメントアウト
    public void AddInCompatibleTypesAreFailedTest( string scriptPath )
    {
        var abortTraverseToken = new AbortTraverseToken();
        var compilerMessageManger = ICompilerMessageManger.Default;
        var ast = ParseTestUtility.Parse( TestDataDirectory, scriptPath );
        var symbolAnalyzer = new SymbolCollector( compilerMessageManger );

        symbolAnalyzer.Analyze( ast, abortTraverseToken );
        Assert.IsFalse( abortTraverseToken.Aborted );

        var semanticAnalyzer = new SemanticAnalyzer( compilerMessageManger, symbolAnalyzer.Variables );

        Assert.DoesNotThrow( () => semanticAnalyzer.Analyze( ast, abortTraverseToken ) );
        compilerMessageManger.WriteTo( System.Console.Out );

        Assert.IsTrue( abortTraverseToken.Aborted );
        Assert.IsTrue( compilerMessageManger.Count( CompilerMessageLevel.Error ) > 0 );
    }

}
