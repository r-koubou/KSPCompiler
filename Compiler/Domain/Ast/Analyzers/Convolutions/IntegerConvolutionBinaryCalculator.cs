using System;

using KSPCompiler.Domain.Ast.Nodes;

namespace KSPCompiler.Domain.Ast.Analyzers.Convolutions;

/// <summary>
/// Calculator for convolution operations with binary operations
/// </summary>
public sealed class IntegerConvolutionBinaryCalculator : IConvolutionBinaryCalculator<int>
{
    private IConvolutionEvaluator<int> EvaluatorForRecursive { get; }

    public IntegerConvolutionBinaryCalculator( IConvolutionEvaluator<int> evaluatorForRecursive )
    {
        EvaluatorForRecursive = evaluatorForRecursive;
    }

    public int? Calculate( AstExpressionSyntaxNode expr, int workingValueForRecursive )
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
            AstNodeId.BitwiseOr   => leftValue | rightValue,
            AstNodeId.BitwiseAnd  => leftValue & rightValue,
            AstNodeId.BitwiseXor  => leftValue ^ rightValue,
            _                     => null
        };
    }
}
