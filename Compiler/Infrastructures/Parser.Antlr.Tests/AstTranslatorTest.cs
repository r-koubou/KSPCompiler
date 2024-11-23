using System.IO;

using KSPCompiler.Domain;
using KSPCompiler.Parser.Antlr.Tests.Commons;

using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace KSPCompiler.Parser.Antlr.Tests;

[TestFixture]
public class AstTranslatorTest
{
    private static readonly string TestDataDirectory = Path.Combine( "TestData", "AstTranslatorTest" );

    // ReSharper disable once UnusedMethodReturnValue.Local

    [Test]
    public void LexerErrorTest()
    {
        Assert.That( () => ParseTestUtility.Parse( TestDataDirectory, "LexerErrorTest.txt" ), Throws.TypeOf<KspScriptParseException>() );
    }

    [Test]
    public void ParserErrorTest()
    {
        Assert.That( () => ParseTestUtility.Parse( TestDataDirectory, "ParserErrorTest.txt" ), Throws.TypeOf<KspScriptParseException>() );
    }

    [Test]
    public void ExpressionTest()
    {
        Assert.That( () => ParseTestUtility.Parse( TestDataDirectory, "ExpressionTest.txt" ), Throws.Nothing );
    }

    [Test]
    public void CallCommandTest()
    {
        Assert.That( () => ParseTestUtility.Parse( TestDataDirectory, "CallCommandTest.txt" ), Throws.Nothing );
    }

    [Test]
    public void AssignTest()
    {
        Assert.That( () => ParseTestUtility.Parse( TestDataDirectory, "AssignTest.txt" ), Throws.Nothing );
    }

    [Test]
    public void StatementTest()
    {
        Assert.That( () => ParseTestUtility.Parse( TestDataDirectory, "StatementTest.txt" ), Throws.Nothing );
    }

}
