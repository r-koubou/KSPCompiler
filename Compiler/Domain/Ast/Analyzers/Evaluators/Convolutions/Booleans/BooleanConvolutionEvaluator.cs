using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Conditions;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Extensions;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;

namespace KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Booleans;

public class BooleanConvolutionEvaluator : IBooleanConvolutionEvaluator
{
    private IAstVisitor Visitor { get; }
    private IBooleanConstantConvolutionCalculator ConstantConvolutionCalculator { get; }
    private IIntegerConditionalBinaryOperatorConvolutionCalculator IntegerBinaryOperatorCalculator { get; }
    private IRealConditionalBinaryOperatorConvolutionCalculator RealBinaryOperatorCalculator { get; }
    private IConditionalLogicalOperatorConvolutionCalculator BooleanLogicalOperatorCalculator { get; }
    private IBooleanConditionalUnaryOperatorConvolutionCalculator BooleanUnaryOperatorCalculator { get; }

    public BooleanConvolutionEvaluator(
        IAstVisitor visitor,
        IIntegerConditionalBinaryOperatorConvolutionCalculator integerConvolutionEvaluator,
        IRealConditionalBinaryOperatorConvolutionCalculator realConvolutionEvaluator )
    {
        Visitor = visitor;

        ConstantConvolutionCalculator    = new BooleanConstantConvolutionCalculator();
        IntegerBinaryOperatorCalculator  = integerConvolutionEvaluator;
        RealBinaryOperatorCalculator     = realConvolutionEvaluator;
        BooleanLogicalOperatorCalculator = new ConditionalLogicalOperatorConvolutionCalculator( visitor, this );
        BooleanUnaryOperatorCalculator   = new BooleanConditionalUnaryOperatorConvolutionCalculator( this );
    }

    public bool? Evaluate( AstExpressionNode expr, bool workingValueForRecursive )
    {
        var id = expr.Id;

        if( expr.ChildNodeCount == 0 )
        {
            return ConstantConvolutionCalculator.Calculate( expr, workingValueForRecursive );
        }

        if( id.IsBooleanSupportedBinaryOperator() )
        {
            return CalculateBinaryOperator( expr );
        }

        if( id.IsBooleanSupportedUnaryOperator() )
        {
            return BooleanUnaryOperatorCalculator.Calculate( expr );
        }

        if( id.IsConditionalLogicalOperator() )
        {
            return BooleanLogicalOperatorCalculator.Calculate( expr );
        }

        return null;
    }

    private bool? CalculateBinaryOperator( AstExpressionNode expr )
    {
        if( expr.Left.Accept( Visitor ) is not AstExpressionNode evaluatedLeft )
        {
            return null;
        }

        if( expr.Right.Accept( Visitor ) is not AstExpressionNode evaluatedRight )
        {
            return null;
        }

        var leftType = evaluatedLeft.TypeFlag;
        var rightType = evaluatedRight.TypeFlag;

        if( leftType != rightType )
        {
            return null;
        }

        if( leftType.IsInt() )
        {
            return IntegerBinaryOperatorCalculator.Calculate( expr );
        }

        if( leftType.IsReal() )
        {
            return RealBinaryOperatorCalculator.Calculate( expr );
        }

        if( leftType.IsBoolean() )
        {
            return BooleanLogicalOperatorCalculator.Calculate( expr );
        }

        return null;
    }
}
