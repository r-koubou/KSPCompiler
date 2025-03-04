using System;

using KSPCompiler.Features.Compilation.UseCase.Abstractions.Evaluations.Convolutions.Booleans;
using KSPCompiler.Shared.Domain.Ast.Nodes;
using KSPCompiler.Shared.Domain.Symbols.MetaData.Extensions;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Commons.Evaluations.Convolutions.Conditions;

public class ConditionalLogicalOperatorConvolutionCalculator : IConditionalLogicalOperatorConvolutionCalculator
{
    private IBooleanConvolutionEvaluator ConvolutionEvaluator { get; }

    public ConditionalLogicalOperatorConvolutionCalculator(
        IBooleanConvolutionEvaluator convolutionEvaluator )
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

        if( !exprLeft.TypeFlag.IsBoolean() || !exprRight.TypeFlag.IsBoolean() ||
            !exprLeft.Constant || !exprRight.Constant )
        {
            return null;
        }

        var convolutedLeft = ConvolutionEvaluator.Evaluate( visitor, exprLeft,   false );
        var convolutedRight = ConvolutionEvaluator.Evaluate( visitor, exprRight, false );

        if( convolutedLeft == null || convolutedRight == null )
        {
            return null;
        }

        return expr.Id switch
        {
            AstNodeId.LogicalAnd => convolutedLeft.Value && convolutedRight.Value,
            AstNodeId.LogicalOr  => convolutedLeft.Value || convolutedRight.Value,
            AstNodeId.LogicalXor => convolutedLeft.Value ^ convolutedRight.Value,
            _                    => null
        };
    }
}
