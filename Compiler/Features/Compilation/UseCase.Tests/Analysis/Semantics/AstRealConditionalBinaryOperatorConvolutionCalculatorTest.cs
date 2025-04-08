using KSPCompiler.Features.Compilation.UseCase.Analysis.Commons.Evaluations.Convolutions.Conditions;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions;
using KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData;

using NUnit.Framework;

namespace KSPCompiler.Features.SymbolManagement.UseCase.Tests.Analysis.Semantics;

[TestFixture]
public class AstRealConditionalBinaryOperatorConvolutionCalculatorTest
{
    private static void ConvolutionConditionalOperatorTestBody<TNode>( double value, bool expected )
        where TNode : AstExpressionNode, new()
    {
        var visitor = new MockDefaultAstVisitor();
        var realConvolutionEvaluator = new MockRealConvolutionEvaluator( value ); // always return `value`

        var calulator = new RealConditionalBinaryOperatorConvolutionCalculator(
            realConvolutionEvaluator
        );

        var ast = new TNode
        {
            TypeFlag = DataTypeFlag.TypeBool,
            Left     = new AstRealLiteralNode( value ),
            Right    = new AstRealLiteralNode( value )
        };

        var result = calulator.Calculate( visitor, ast );

        Assert.That( result, Is.Not.Null );
        Assert.That( result, Is.EqualTo( expected ) );
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
