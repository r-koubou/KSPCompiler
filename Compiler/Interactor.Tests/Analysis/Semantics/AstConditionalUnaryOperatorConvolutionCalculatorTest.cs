using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Interactor.Analysis.Commons.Evaluations.Convolutions.Booleans;

using NUnit.Framework;

namespace KSPCompiler.Interactor.Tests.Analysis.Semantics;

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

        Assert.IsNotNull( result );
        Assert.AreEqual( !value, result );
    }
}
