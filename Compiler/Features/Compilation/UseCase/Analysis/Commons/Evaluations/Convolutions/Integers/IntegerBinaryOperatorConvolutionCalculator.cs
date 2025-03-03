using System;

using KSPCompiler.Features.Compilation.Domain.Ast.Nodes;
using KSPCompiler.Features.Compilation.UseCase.Abstractions.Evaluations.Convolutions.Integers;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Commons.Evaluations.Convolutions.Integers;

/// <summary>
/// Calculator for convolution operations with binary operators (integer)
/// </summary>
public sealed class IntegerBinaryOperatorConvolutionCalculator : IIntegerBinaryOperatorConvolutionCalculator
{
    private IIntegerConvolutionEvaluator EvaluatorForRecursive { get; }

    public IntegerBinaryOperatorConvolutionCalculator( IIntegerConvolutionEvaluator evaluatorForRecursive )
    {
        EvaluatorForRecursive = evaluatorForRecursive;
    }

    public int? Calculate( IAstVisitor visitor, AstExpressionNode expr, int workingValueForRecursive )
    {
        if( expr.ChildNodeCount != 2 )
        {
            throw new ArgumentException( $"Expected 2 child nodes, but got {expr.ChildNodeCount}. (node: {expr.GetType().Name})" );
        }

        var leftValue = EvaluatorForRecursive.Evaluate( visitor, expr.Left, workingValueForRecursive );
        if( leftValue == null )
        {
            return null;
        }

        var rightValue = EvaluatorForRecursive.Evaluate( visitor, expr.Right, workingValueForRecursive );
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
