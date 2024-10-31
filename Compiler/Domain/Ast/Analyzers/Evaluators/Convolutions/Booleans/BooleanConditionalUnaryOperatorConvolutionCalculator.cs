using System;

using KSPCompiler.Domain.Ast.Nodes;

namespace KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Booleans;

/// <summary>
/// Calculator for convolution operations with conditional unary operators (boolean)
/// </summary>
public class BooleanConditionalUnaryOperatorConvolutionCalculator : IBooleanConditionalUnaryOperatorConvolutionCalculator
{
    private IPrimitiveConvolutionEvaluator<bool> EvaluatorForRecursive { get; }

    public BooleanConditionalUnaryOperatorConvolutionCalculator( IPrimitiveConvolutionEvaluator<bool> evaluatorForRecursive )
    {
        EvaluatorForRecursive = evaluatorForRecursive;
    }

    public bool? Calculate( AstExpressionNode expr )
    {
        if( expr.ChildNodeCount != 1 )
        {
            throw new ArgumentException( $"Expected 1 child node, but got {expr.ChildNodeCount}. (node: {expr.GetType().Name})" );
        }

        var convolutedValue = EvaluatorForRecursive.Evaluate( expr.Left, false );

        if( convolutedValue == null )
        {
            return null;
        }

        return expr.Id switch
        {
            AstNodeId.UnaryLogicalNot => !convolutedValue,
            _                         => null
        };
    }
}
