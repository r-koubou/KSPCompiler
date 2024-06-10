using System.IO;

using KSPCompiler.Domain;
using KSPCompiler.Parser.Antlr.Tests.Commons;

using NUnit.Framework;

namespace KSPCompiler.Parser.Antlr.Tests;

[TestFixture]
public class AstTranslatorTest
{
    private static readonly string TestDataDirectory = Path.Combine( "TestData", "AstTranslatorTest" );

    // ReSharper disable once UnusedMethodReturnValue.Local

    [Test]
    public void LexerErrorTest()
    {
        Assert.Throws<KspScriptParseException>( () => ParseTestUtility.Parse( TestDataDirectory, "LexerErrorTest.txt" ) );
    }

    [Test]
    public void ParserErrorTest()
    {
        Assert.Throws<KspScriptParseException>( () => ParseTestUtility.Parse( TestDataDirectory, "ParserErrorTest.txt" ) );
    }

    [Test]
    public void ExpressionTest()
    {
        Assert.DoesNotThrow( () => ParseTestUtility.Parse( TestDataDirectory, "ExpressionTest.txt" ) );
    }

    [Test]
    public void CallCommandTest()
    {
        Assert.DoesNotThrow( () => ParseTestUtility.Parse( TestDataDirectory, "CallCommandTest.txt" ) );
    }

    [Test]
    public void AssignTest()
    {
        Assert.DoesNotThrow( () => ParseTestUtility.Parse( TestDataDirectory, "AssignTest.txt" ) );
    }

    [Test]
    public void StatementTest()
    {
        Assert.DoesNotThrow( () => ParseTestUtility.Parse( TestDataDirectory, "StatementTest.txt" ) );
    }

}
