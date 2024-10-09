using System;

using KSPCompiler.Domain.Ast.Nodes;

namespace KSPCompiler.Domain.Ast.Analyzers.Convolutions;

/// <summary>
/// Calculator for convolution operations with unary operations
/// </summary>
public sealed class IntegerConvolutionUnaryCalculator : IConvolutionUnaryCalculator<int>
{
    private IConvolutionEvaluator<int> EvaluatorForRecursive { get; }

    public IntegerConvolutionUnaryCalculator( IConvolutionEvaluator<int> evaluatorForRecursive )
    {
        EvaluatorForRecursive = evaluatorForRecursive;
    }

    public int? Calculate( AstExpressionSyntaxNode expr, int workingValueForRecursive )
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
            AstNodeId.UnaryNot        => ~convolutedValue,
            AstNodeId.UnaryLogicalNot => convolutedValue != 0 ? 0 : 1, // C言語と同じ、0=false, 0以外が真をベースにしている
            _ => null
        };
    }
}
