using System;

using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Conditions;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Symbols.MetaData;

using NUnit.Framework;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

[TestFixture]
public class AstConditionalBinaryOperatorConvolutionCalculatorTest
{
    private static void ConvolutionIntegerConditionalOperatorTestBody<TNode>( int value, bool expected )
        where TNode : AstExpressionNode, new()
    {
        var visitor = new MockDefaultAstVisitor();
        var integerConvolutionEvaluator = new MockIntegerConvolutionEvaluator( value ); // always return `value`

        var calulator = new IntegerConditionalBinaryOperatorConvolutionCalculator(
            visitor,
            integerConvolutionEvaluator
        );

        var ast = new TNode
        {
            TypeFlag = DataTypeFlag.TypeBool,
            Left     = new AstIntLiteralNode( value ),
            Right    = new AstIntLiteralNode( value )
        };

        var result = calulator.Calculate( ast );

        Assert.IsNotNull( result );
        Assert.AreEqual( expected, result );
    }

    [Test]
    public void ConvolutionIntegerEqualTest()
    {
        // 1 == 1  --> true
        ConvolutionIntegerConditionalOperatorTestBody<AstEqualExpressionNode>( 1, true );
    }

    [Test]
    public void ConvolutionIntegerNotEqualTest()
    {
        // 1 != 1  --> false
        ConvolutionIntegerConditionalOperatorTestBody<AstNotEqualExpressionNode>( 1, false );
    }

    [Test]
    public void ConvolutionIntegerLessThanEqualTest()
    {
        // 1 < 1  --> false
        ConvolutionIntegerConditionalOperatorTestBody<AstLessThanExpressionNode>( 1, false );
    }

    [Test]
    public void ConvolutionIntegerGreaterThanEqualTest()
    {
        // 1 > 1  --> false
        ConvolutionIntegerConditionalOperatorTestBody<AstGreaterThanExpressionNode>( 1, false );
    }

    [Test]
    public void ConvolutionIntegerLessEqualEqualTest()
    {
        // 1 <= 1  --> true
        ConvolutionIntegerConditionalOperatorTestBody<AstLessEqualExpressionNode>( 1, true );
    }

    [Test]
    public void ConvolutionIntegerGreaterEqualEqualTest()
    {
        // 1 >= 1  --> true
        ConvolutionIntegerConditionalOperatorTestBody<AstGreaterEqualExpressionNode>( 1, true );
    }

}
