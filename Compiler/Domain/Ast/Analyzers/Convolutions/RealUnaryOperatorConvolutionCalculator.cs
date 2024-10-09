using System;

using KSPCompiler.Domain.Ast.Nodes;

namespace KSPCompiler.Domain.Ast.Analyzers.Convolutions;

/// <summary>
/// Calculator for convolution operations with unary operators
/// </summary>
public sealed class RealUnaryOperatorConvolutionCalculator : IPrimitiveConvolutionUnaryCalculator<double>
{
    private IPrimitiveConvolutionEvaluator<double> EvaluatorForRecursive { get; }

    public RealUnaryOperatorConvolutionCalculator( IPrimitiveConvolutionEvaluator<double> evaluatorForRecursive )
    {
        EvaluatorForRecursive = evaluatorForRecursive;
    }

    public double? Calculate( AstExpressionSyntaxNode expr, double workingValueForRecursive )
    {
        if( expr.ChildNodeCount != 1 )
        {
            throw new ArgumentException( $"Expected 1 child node, but got {expr.ChildNodeCount}. (node: {expr.GetType().Name})" );
        }

        var convolutedValue = EvaluatorForRecursive.Evaluate( expr.Left, workingValueForRecursive );

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