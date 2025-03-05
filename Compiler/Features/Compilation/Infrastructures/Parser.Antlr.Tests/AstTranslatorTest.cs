using System.IO;

using KSPCompiler.Features.Compilation.Infrastructures.Parser.Antlr.Tests.Commons;

using NUnit.Framework;

namespace KSPCompiler.Features.Compilation.Infrastructures.Parser.Antlr.Tests;

[TestFixture]
public class AstTranslatorTest
{
    private static readonly string TestDataDirectory = Path.Combine( "TestData", "AstTranslatorTest" );

    // ReSharper disable once UnusedMethodReturnValue.Local

    [Test]
    public void LexerFallbackTest()
    {
        Assert.That( () => ParseTestUtility.Parse( TestDataDirectory, "LexerErrorTest.txt" ), Throws.Nothing );
    }

    [Test]
    public void ParserFallbackTest()
    {
        Assert.That( () => ParseTestUtility.Parse( TestDataDirectory, "ParserErrorTest.txt" ), Throws.Nothing );
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
