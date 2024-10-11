using System;

using KSPCompiler.Domain.Ast.Nodes;

namespace KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Reals;

/// <summary>
/// Calculator for convolution operations with binary operators
/// </summary>
public sealed class RealBinaryOperatorConvolutionCalculator : IRealBinaryOperatorConvolutionCalculator
{
    private IRealConvolutionEvaluator EvaluatorForRecursive { get; }

    public RealBinaryOperatorConvolutionCalculator( IRealConvolutionEvaluator evaluatorForRecursive )
    {
        EvaluatorForRecursive = evaluatorForRecursive;
    }

    public double? Calculate( AstExpressionNode expr, double workingValueForRecursive )
    {
        if( expr.ChildNodeCount != 2 )
        {
            throw new ArgumentException( $"Expected 2 child nodes, but got {expr.ChildNodeCount}. (node: {expr.GetType().Name})" );
        }

        var leftValue = EvaluatorForRecursive.Evaluate( expr.Left, workingValueForRecursive );
        if( leftValue == null )
        {
            return null;
        }

        var rightValue = EvaluatorForRecursive.Evaluate( expr.Right, workingValueForRecursive );
        if( rightValue == null )
        {
            return null;
        }

        return expr.Id switch
        {
            AstNodeId.Addition    => leftValue + rightValue,
            AstNodeId.Subtraction => leftValue - rightValue,
            AstNodeId.Multiplying => leftValue * rightValue,
            AstNodeId.Division    => leftValue / rightValue,
            AstNodeId.Modulo      => leftValue % rightValue,
            _                     => null
        };
    }
}
