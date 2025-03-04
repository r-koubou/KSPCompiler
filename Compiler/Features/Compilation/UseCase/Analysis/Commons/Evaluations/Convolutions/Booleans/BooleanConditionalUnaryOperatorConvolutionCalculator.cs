using System;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Convolutions;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Convolutions.Booleans;
using KSPCompiler.Shared.Domain.Ast.Nodes;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Commons.Evaluations.Convolutions.Booleans;

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

    public bool? Calculate( IAstVisitor visitor, AstExpressionNode expr )
    {
        if( expr.ChildNodeCount != 1 )
        {
            throw new ArgumentException( $"Expected 1 child node, but got {expr.ChildNodeCount}. (node: {expr.GetType().Name})" );
        }

        var convolutedValue = EvaluatorForRecursive.Evaluate( visitor, expr.Left, false );

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
