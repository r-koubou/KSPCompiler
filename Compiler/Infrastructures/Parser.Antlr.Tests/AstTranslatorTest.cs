using System.IO;
using System.Text;

using KSPCompiler.Domain;
using KSPCompiler.Domain.Ast.Node.Blocks;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Infrastructures.Parser.Antlr;

using NUnit.Framework;

namespace KSPCompiler.Parser.Antlr.Tests;

[TestFixture]
public class AstTranslatorTest
{
    private static readonly string TestDataDirectory = Path.Combine( "TestData", "AstTranslatorTest" );

    // ReSharper disable once UnusedMethodReturnValue.Local
    private static AstCompilationUnit TranslateImpl( string scriptFilePath )
    {
        var path = Path.Combine(
            TestContext.CurrentContext.TestDirectory,
            TestDataDirectory,
            scriptFilePath
        );

        var p = new KspFileSyntaxAnalyser( path, ICompilerMessageManger.CreateDefault(), Encoding.UTF8 );
        var result = p.Analyse();
        return result;
    }

    [Test]
    public void LexerErrorTest()
    {
        Assert.Throws<KspScriptParseException>( () => TranslateImpl( "LexerErrorTest.txt" ) );
    }

    [Test]
    public void ParserErrorTest()
    {
        Assert.Throws<KspScriptParseException>( () => TranslateImpl( "ParserErrorTest.txt" ) );
    }

    [Test]
    public void ExpressionTest()
    {
        Assert.DoesNotThrow( () => TranslateImpl( "ExpressionTest.txt" ) );
    }

    [Test]
    public void CallCommandTest()
    {
        Assert.DoesNotThrow( () => TranslateImpl( "CallCommandTest.txt" ) );
    }

    [Test]
    public void AssignTest()
    {
        Assert.DoesNotThrow( () => TranslateImpl( "AssignTest.txt" ) );
    }

    [Test]
    public void StatementTest()
    {
        Assert.DoesNotThrow( () => TranslateImpl( "StatementTest.txt" ) );
    }

}
