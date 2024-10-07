using System;

using KSPCompiler.Domain.Ast.Nodes;

namespace KSPCompiler.Domain.Ast.Analyzers.Convolutions;

/// <summary>
/// Calculator for convolution operations with unary operations
/// </summary>
public sealed class RealConvolutionUnaryCalculator : IConvolutionUnaryCalculator<double>
{
    private IConvolutionEvaluator<double> EvaluatorForRecursive { get; }

    public RealConvolutionUnaryCalculator( IConvolutionEvaluator<double> evaluatorForRecursive )
    {
        EvaluatorForRecursive = evaluatorForRecursive;
    }

    public double? Calculate( AstExpressionSyntaxNode expr, double workingValueForRecursive )
    {
        if( expr.ChildNodeCount != 1 )
        {
            throw new ArgumentException( $"Expected 1 child node, but got {expr.ChildNodeCount}. (node: {expr.GetType().Name})" );
        }

        var unary = expr.Left;
        var convolutedValue = EvaluatorForRecursive.Evaluate( expr, workingValueForRecursive );

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
