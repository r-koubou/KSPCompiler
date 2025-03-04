using System;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Convolutions.Strings;
using KSPCompiler.Shared.Domain.Ast.Nodes;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Commons.Evaluations.Convolutions.Strings;

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

    public string? Calculate( IAstVisitor visitor, AstExpressionNode expr, string workingValueForRecursive )
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
            AstNodeId.StringConcatenate => leftValue + rightValue,
            _                           => null
        };
    }
}
