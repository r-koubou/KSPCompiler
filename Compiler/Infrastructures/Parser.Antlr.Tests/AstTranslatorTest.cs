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
        ClassicAssert.Throws<KspScriptParseException>( () => ParseTestUtility.Parse( TestDataDirectory, "LexerErrorTest.txt" ) );
    }

    [Test]
    public void ParserErrorTest()
    {
        ClassicAssert.Throws<KspScriptParseException>( () => ParseTestUtility.Parse( TestDataDirectory, "ParserErrorTest.txt" ) );
    }

    [Test]
    public void ExpressionTest()
    {
        ClassicAssert.DoesNotThrow( () => ParseTestUtility.Parse( TestDataDirectory, "ExpressionTest.txt" ) );
    }

    [Test]
    public void CallCommandTest()
    {
        ClassicAssert.DoesNotThrow( () => ParseTestUtility.Parse( TestDataDirectory, "CallCommandTest.txt" ) );
    }

    [Test]
    public void AssignTest()
    {
        ClassicAssert.DoesNotThrow( () => ParseTestUtility.Parse( TestDataDirectory, "AssignTest.txt" ) );
    }

    [Test]
    public void StatementTest()
    {
        ClassicAssert.DoesNotThrow( () => ParseTestUtility.Parse( TestDataDirectory, "StatementTest.txt" ) );
    }

}
