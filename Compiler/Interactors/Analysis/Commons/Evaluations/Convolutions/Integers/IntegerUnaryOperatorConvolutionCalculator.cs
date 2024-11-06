using System;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.UseCases.Analysis.Evaluations.Convolutions;
using KSPCompiler.UseCases.Analysis.Evaluations.Convolutions.Integers;

namespace KSPCompiler.Interactors.Analysis.Commons.Evaluations.Convolutions.Integers;

/// <summary>
/// Calculator for convolution operations with unary operators
/// </summary>
public sealed class IntegerUnaryOperatorConvolutionCalculator : IIntegerUnaryOperatorConvolutionCalculator
{
    private IPrimitiveConvolutionEvaluator<int> EvaluatorForRecursive { get; }

    public IntegerUnaryOperatorConvolutionCalculator( IPrimitiveConvolutionEvaluator<int> evaluatorForRecursive )
    {
        EvaluatorForRecursive = evaluatorForRecursive;
    }

    public int? Calculate( IAstVisitor visitor, AstExpressionNode expr, int workingValueForRecursive )
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
            AstNodeId.UnaryNot        => ~convolutedValue,
            _ => null
        };
    }
}
