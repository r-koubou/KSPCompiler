using System;

using KSPCompiler.Domain.Ast.Nodes;

namespace KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Reals;

/// <summary>
/// Calculator for convolution operations with unary operators
/// </summary>
public sealed class RealUnaryOperatorConvolutionCalculator : IRealUnaryOperatorConvolutionCalculator
{
    private IRealConvolutionEvaluator EvaluatorForRecursive { get; }

    public RealUnaryOperatorConvolutionCalculator( IRealConvolutionEvaluator evaluatorForRecursive )
    {
        EvaluatorForRecursive = evaluatorForRecursive;
    }

    public double? Calculate( IAstVisitor visitor, AstExpressionNode expr, double workingValueForRecursive )
    {
        if( expr.ChildNodeCount != 1 )
        {
            throw new ArgumentException( $"Expected 1 child node, but got {expr.ChildNodeCount}. (node: {expr.GetType().Name})" );
        }

        var convolutedValue = EvaluatorForRecursive.Evaluate( visitor, expr.Left, workingValueForRecursive );

        if( convolutedValue == null )
        {
            return null;
        }

        return expr.Id switch
        {
            AstNodeId.UnaryMinus     => -convolutedValue,
            _ => null
        };
    }
}
