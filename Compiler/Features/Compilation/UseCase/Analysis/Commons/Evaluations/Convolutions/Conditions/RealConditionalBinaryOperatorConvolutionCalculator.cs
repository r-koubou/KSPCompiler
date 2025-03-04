using System;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Convolutions.Conditions;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Convolutions.Reals;
using KSPCompiler.Shared.Domain.Ast.Nodes;
using KSPCompiler.Shared.Domain.Symbols.MetaData.Extensions;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Commons.Evaluations.Convolutions.Conditions;

/// <summary>
/// Calculator for convolution operations with conditional operators
/// </summary>
public sealed class RealConditionalBinaryOperatorConvolutionCalculator : IRealConditionalBinaryOperatorConvolutionCalculator
{
    private IRealConvolutionEvaluator ConvolutionEvaluator { get; }

    public RealConditionalBinaryOperatorConvolutionCalculator(
        IRealConvolutionEvaluator convolutionEvaluator )
    {
        ConvolutionEvaluator = convolutionEvaluator;
    }

    public bool? Calculate( IAstVisitor visitor, AstExpressionNode expr )
    {
        if( expr.ChildNodeCount != 2 )
        {
            throw new ArgumentException( $"Expected 2 child nodes, but got {expr.ChildNodeCount}. (node: {expr.GetType().Name})" );
        }

        var exprLeft = expr.Left.Accept( visitor ) as AstExpressionNode;
        var exprRight = expr.Right.Accept( visitor ) as AstExpressionNode;

        if( exprLeft == null || exprRight == null )
        {
            return null;
        }

        if( !exprLeft.TypeFlag.IsReal() || !exprRight.TypeFlag.IsReal() ||
            !exprLeft.Constant || !exprRight.Constant )
        {
            return null;
        }

        var convolutedLeft = ConvolutionEvaluator.Evaluate( visitor, exprLeft,   0 );
        var convolutedRight = ConvolutionEvaluator.Evaluate( visitor, exprRight, 0 );

        if( convolutedLeft == null || convolutedRight == null )
        {
            return null;
        }

        return expr.Id switch
        {
            AstNodeId.Equal        => convolutedLeft.Equals( convolutedRight ),
            AstNodeId.NotEqual     => !convolutedLeft.Equals( convolutedRight ),
            AstNodeId.GreaterThan  => convolutedLeft > convolutedRight,
            AstNodeId.GreaterEqual => convolutedLeft >= convolutedRight,
            AstNodeId.LessThan     => convolutedLeft < convolutedRight,
            AstNodeId.LessEqual    => convolutedLeft <= convolutedRight,
            _                      => null
        };
    }
}
