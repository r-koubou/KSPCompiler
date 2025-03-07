using System;

using KSPCompiler.Features.Compilation.Domain.Messages;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Commons.Evaluations.Convolutions.Strings;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions;

using NUnit.Framework;

namespace KSPCompiler.Features.Compilation.UseCase.Tests.Analysis.Semantics;

[TestFixture]
public class AstStringConcatenateOperatorConvolutionCalculatorTest
{
    #region Common test methods
    private static void ConvolutionTestBody(
        AstExpressionNode left,
        AstExpressionNode right,
        string? expectedValue,
        int expectedErrorCount )
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var visitor = new MockDefaultAstVisitor();

        var evaluator = new StringConvolutionEvaluator();

        var operatorNode = new AstStringConcatenateExpressionNode
        {
            Left  = left,
            Right = right
        };

        var result = evaluator.Evaluate( visitor, operatorNode, "" );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count(), Is.EqualTo( expectedErrorCount ) );
        Assert.That( result,                        Is.EqualTo( expectedValue ) );
    }

    #endregion

    [Test]
    public void StringConvolutionTest()
    {
        // "abc" & "def"
        // -> "abcdef"
        ConvolutionTestBody(
            new AstStringLiteralNode( "abc" ),
            new AstStringLiteralNode( "def" ),
            "abcdef",
            0
        );
    }

    [Test]
    public void StringAndIntegerConvolutionTest()
    {
        // "abc" & 123
        // -> "abc123"
        ConvolutionTestBody(
            new AstStringLiteralNode( "abc" ),
            new AstIntLiteralNode( 123 ),
            "abc123",
            0
        );
    }

    [Test]
    public void StringAndRealConvolutionTest()
    {
        // "abc" & 1.23
        // -> "abc1.23"
        ConvolutionTestBody(
            new AstStringLiteralNode( "abc" ),
            new AstRealLiteralNode( 1.23 ),
            "abc1.23",
            0
        );
    }

    [Test]
    public void IntegerAndIntegerConvolutionTest()
    {
        // 123 & 456
        // -> "123456"
        ConvolutionTestBody(
            new AstIntLiteralNode( 123 ),
            new AstIntLiteralNode( 456 ),
            "123456",
            0
        );
    }

    [Test]
    public void IntegerAndRealConvolutionTest()
    {
        // 123 & 4.567
        // -> "1234.567"
        ConvolutionTestBody(
            new AstIntLiteralNode( 123 ),
            new AstRealLiteralNode( 4.567 ),
            "1234.567",
            0
        );
    }

    [Test]
    public void RealAndRealConvolutionTest()
    {
        // 1.23 & 4.56
        // -> "1.234.56"
        ConvolutionTestBody(
            new AstRealLiteralNode( 1.23 ),
            new AstRealLiteralNode( 4.56 ),
            "1.234.56",
            0
        );
    }

    [Test]
    public void CannotBooleanConvolutionTest()
    {
        // ( 1 > 0 ) & "abc" <-- cannot concatenate boolean
        ConvolutionTestBody(
            new AstBooleanLiteralNode( true ),
            new AstStringLiteralNode( "abc" ),
            null,
            0
        );
    }

}
