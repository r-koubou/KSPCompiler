using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Convolutions.Booleans;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Convolutions.Conditions;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Commons.Evaluations.Convolutions.Conditions;
using KSPCompiler.Shared.Domain.Ast.Nodes;
using KSPCompiler.Shared.Domain.Ast.Nodes.Extensions;
using KSPCompiler.Shared.Domain.Symbols.MetaData.Extensions;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Commons.Evaluations.Convolutions.Booleans;

public class BooleanConvolutionEvaluator : IBooleanConvolutionEvaluator
{
    private IBooleanConstantConvolutionCalculator ConstantConvolutionCalculator { get; }
    private IIntegerConditionalBinaryOperatorConvolutionCalculator IntegerBinaryOperatorCalculator { get; }
    private IRealConditionalBinaryOperatorConvolutionCalculator RealBinaryOperatorCalculator { get; }
    private IConditionalLogicalOperatorConvolutionCalculator BooleanLogicalOperatorCalculator { get; }
    private IBooleanConditionalUnaryOperatorConvolutionCalculator BooleanUnaryOperatorCalculator { get; }

    public BooleanConvolutionEvaluator(
        IIntegerConditionalBinaryOperatorConvolutionCalculator integerConvolutionEvaluator,
        IRealConditionalBinaryOperatorConvolutionCalculator realConvolutionEvaluator )
    {
        ConstantConvolutionCalculator    = new BooleanConstantConvolutionCalculator();
        IntegerBinaryOperatorCalculator  = integerConvolutionEvaluator;
        RealBinaryOperatorCalculator     = realConvolutionEvaluator;
        BooleanLogicalOperatorCalculator = new ConditionalLogicalOperatorConvolutionCalculator( this );
        BooleanUnaryOperatorCalculator   = new BooleanConditionalUnaryOperatorConvolutionCalculator( this );
    }

    public bool? Evaluate( IAstVisitor visitor, AstExpressionNode expr, bool workingValueForRecursive )
    {
        var id = expr.Id;

        if( expr.ChildNodeCount == 0 )
        {
            return ConstantConvolutionCalculator.Calculate( visitor, expr, workingValueForRecursive );
        }

        if( id.IsBooleanSupportedBinaryOperator() )
        {
            return CalculateBinaryOperator( visitor, expr );
        }

        if( id.IsBooleanSupportedUnaryOperator() )
        {
            return BooleanUnaryOperatorCalculator.Calculate( visitor, expr );
        }

        if( id.IsConditionalLogicalOperator() )
        {
            return BooleanLogicalOperatorCalculator.Calculate( visitor, expr );
        }

        return null;
    }

    private bool? CalculateBinaryOperator( IAstVisitor visitor, AstExpressionNode expr )
    {
        if( expr.Left.Accept( visitor ) is not AstExpressionNode evaluatedLeft )
        {
            return null;
        }

        if( expr.Right.Accept( visitor ) is not AstExpressionNode evaluatedRight )
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
            return IntegerBinaryOperatorCalculator.Calculate( visitor, expr );
        }

        if( leftType.IsReal() )
        {
            return RealBinaryOperatorCalculator.Calculate( visitor, expr );
        }

        if( leftType.IsBoolean() )
        {
            return BooleanLogicalOperatorCalculator.Calculate( visitor, expr );
        }

        return null;
    }
}
