using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Conditions;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Symbols.MetaData;

using NUnit.Framework;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

[TestFixture]
public class AstRealConditionalBinaryOperatorConvolutionCalculatorTest
{
    private static void ConvolutionConditionalOperatorTestBody<TNode>( double value, bool expected )
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
    public void ConvolutionEqualTest()
    {
        // 1.0 == 1.0  --> true
        ConvolutionConditionalOperatorTestBody<AstEqualExpressionNode>( 1.0, true );
    }

    [Test]
    public void ConvolutionNotEqualTest()
    {
        // 1.0 != 1.0  --> false
        ConvolutionConditionalOperatorTestBody<AstNotEqualExpressionNode>( 1.0, false );
    }

    [Test]
    public void ConvolutionLessThanEqualTest()
    {
        // 1.0 < 1.0  --> false
        ConvolutionConditionalOperatorTestBody<AstLessThanExpressionNode>( 1.0, false );
    }

    [Test]
    public void ConvolutionGreaterThanEqualTest()
    {
        // 1.0 > 1.0  --> false
        ConvolutionConditionalOperatorTestBody<AstGreaterThanExpressionNode>( 1.0, false );
    }

    [Test]
    public void ConvolutionLessEqualEqualTest()
    {
        // 1.0 <= 1.0  --> true
        ConvolutionConditionalOperatorTestBody<AstLessEqualExpressionNode>( 1.0, true );
    }

    [Test]
    public void ConvolutionGreaterEqualEqualTest()
    {
        // 1.0 >= 1.0  --> true
        ConvolutionConditionalOperatorTestBody<AstGreaterEqualExpressionNode>( 1.0, true );
    }

}
