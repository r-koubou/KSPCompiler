using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Conditions;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Symbols.MetaData;

using NUnit.Framework;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

[TestFixture]
public class AstRealConditionalBinaryOperatorConvolutionCalculatorTest
{
    private static void ConvolutionIntegerConditionalOperatorTestBody<TNode>( double value, bool expected )
        where TNode : AstExpressionNode, new()
    {
        var visitor = new MockDefaultAstVisitor();
        var realConvolutionEvaluator = new MockRealConvolutionEvaluator( value ); // always return `value`

        var calulator = new RealConditionalBinaryOperatorConvolutionCalculator(
            visitor,
            realConvolutionEvaluator
        );

        var ast = new TNode
        {
            TypeFlag = DataTypeFlag.TypeBool,
            Left     = new AstRealLiteralNode( value ),
            Right    = new AstRealLiteralNode( value )
        };

        var result = calulator.Calculate( ast );

        Assert.IsNotNull( result );
        Assert.AreEqual( expected, result );
    }

    [Test]
    public void ConvolutionIntegerEqualTest()
    {
        // 1.0 == 1.0  --> true
        ConvolutionIntegerConditionalOperatorTestBody<AstEqualExpressionNode>( 1.0, true );
    }

    [Test]
    public void ConvolutionIntegerNotEqualTest()
    {
        // 1.0 != 1.0  --> false
        ConvolutionIntegerConditionalOperatorTestBody<AstNotEqualExpressionNode>( 1.0, false );
    }

    [Test]
    public void ConvolutionIntegerLessThanEqualTest()
    {
        // 1.0 < 1.0  --> false
        ConvolutionIntegerConditionalOperatorTestBody<AstLessThanExpressionNode>( 1.0, false );
    }

    [Test]
    public void ConvolutionIntegerGreaterThanEqualTest()
    {
        // 1.0 > 1.0  --> false
        ConvolutionIntegerConditionalOperatorTestBody<AstGreaterThanExpressionNode>( 1.0, false );
    }

    [Test]
    public void ConvolutionIntegerLessEqualEqualTest()
    {
        // 1.0 <= 1.0  --> true
        ConvolutionIntegerConditionalOperatorTestBody<AstLessEqualExpressionNode>( 1.0, true );
    }

    [Test]
    public void ConvolutionIntegerGreaterEqualEqualTest()
    {
        // 1.0 >= 1.0  --> true
        ConvolutionIntegerConditionalOperatorTestBody<AstGreaterEqualExpressionNode>( 1.0, true );
    }

}
