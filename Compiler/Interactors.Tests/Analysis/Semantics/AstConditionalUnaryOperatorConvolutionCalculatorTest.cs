using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Interactors.Analysis.Commons.Evaluations.Convolutions.Booleans;

using NUnit.Framework;

namespace KSPCompiler.Interactors.Tests.Analysis.Semantics;

[TestFixture]
public class AstConditionalUnaryOperatorConvolutionCalculatorTest
{
    [TestCase( true )]
    [TestCase( false )]
    public void ConvolutionIntegerConditionalOperatorTest( bool value )
    {
        var visitor = new MockDefaultAstVisitor();
        var integerConvolutionEvaluator = new MockBooleanConvolutionEvaluator( value ); // always return `value`

        var calculator = new BooleanConditionalUnaryOperatorConvolutionCalculator(
            integerConvolutionEvaluator
        );

        var ast = new AstUnaryLogicalNotExpressionNode
        {
            TypeFlag = DataTypeFlag.TypeBool,
            Left     = new AstBooleanLiteralNode( value )
        };

        var result = calculator.Calculate( visitor, ast );

        Assert.That( result, Is.Not.Null );
        Assert.That( result, Is.EqualTo( !value ) );
    }
}
