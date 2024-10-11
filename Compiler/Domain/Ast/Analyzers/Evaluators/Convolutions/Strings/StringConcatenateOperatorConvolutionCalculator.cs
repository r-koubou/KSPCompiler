using System;

using KSPCompiler.Domain.Ast.Nodes;

namespace KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Strings;

/// <summary>
/// Calculator for convolution operations with string concatenation operator (string)
/// </summary>
public sealed class StringConcatenateOperatorConvolutionCalculator : IStringConcatenateOperatorConvolutionCalculator
{
    private IStringConvolutionEvaluator EvaluatorForRecursive { get; }

    public StringConcatenateOperatorConvolutionCalculator( IStringConvolutionEvaluator evaluatorForRecursive )
    {
        EvaluatorForRecursive = evaluatorForRecursive;
    }

    public string? Calculate( AstExpressionNode expr, string workingValueForRecursive )
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
            AstNodeId.StringConcatenate => leftValue + rightValue,
            _                           => null
        };
    }
}
