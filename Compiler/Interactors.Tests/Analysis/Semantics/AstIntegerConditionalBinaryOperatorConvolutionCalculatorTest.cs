using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Interactors.Analysis.Commons.Evaluations.Convolutions.Conditions;

using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace KSPCompiler.Interactors.Tests.Analysis.Semantics;

[TestFixture]
public class AstIntegerConditionalBinaryOperatorConvolutionCalculatorTest
{
    private static void ConvolutionConditionalOperatorTestBody<TNode>( int value, bool expected )
        where TNode : AstExpressionNode, new()
    {
        var visitor = new MockDefaultAstVisitor();
        var integerConvolutionEvaluator = new MockIntegerConvolutionEvaluator( value ); // always return `value`

        var calulator = new IntegerConditionalBinaryOperatorConvolutionCalculator( integerConvolutionEvaluator );

        var ast = new TNode
        {
            TypeFlag = DataTypeFlag.TypeBool,
            Left     = new AstIntLiteralNode( value ),
            Right    = new AstIntLiteralNode( value )
        };

        var result = calulator.Calculate( visitor, ast );

        Assert.That( result, Is.Not.Null );
        Assert.That( result, Is.EqualTo( expected ) );
    }

    [Test]
    public void ConvolutionEqualTest()
    {
        // 1 == 1  --> true
        ConvolutionConditionalOperatorTestBody<AstEqualExpressionNode>( 1, true );
    }

    [Test]
    public void ConvolutionNotEqualTest()
    {
        // 1 != 1  --> false
        ConvolutionConditionalOperatorTestBody<AstNotEqualExpressionNode>( 1, false );
    }

    [Test]
    public void ConvolutionLessThanEqualTest()
    {
        // 1 < 1  --> false
        ConvolutionConditionalOperatorTestBody<AstLessThanExpressionNode>( 1, false );
    }

    [Test]
    public void ConvolutionGreaterThanEqualTest()
    {
        // 1 > 1  --> false
        ConvolutionConditionalOperatorTestBody<AstGreaterThanExpressionNode>( 1, false );
    }

    [Test]
    public void ConvolutionLessEqualEqualTest()
    {
        // 1 <= 1  --> true
        ConvolutionConditionalOperatorTestBody<AstLessEqualExpressionNode>( 1, true );
    }

    [Test]
    public void ConvolutionGreaterEqualEqualTest()
    {
        // 1 >= 1  --> true
        ConvolutionConditionalOperatorTestBody<AstGreaterEqualExpressionNode>( 1, true );
    }

}
