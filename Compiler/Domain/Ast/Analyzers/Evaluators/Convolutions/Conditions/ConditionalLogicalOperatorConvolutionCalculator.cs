using System;

using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Booleans;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;

namespace KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Conditions;

public class ConditionalLogicalOperatorConvolutionCalculator : IConditionalLogicalOperatorConvolutionCalculator
{
    private IAstVisitor Visitor { get; }
    private IBooleanConvolutionEvaluator ConvolutionEvaluator { get; }

    public ConditionalLogicalOperatorConvolutionCalculator(
        IAstVisitor visitor,
        IBooleanConvolutionEvaluator convolutionEvaluator )
    {
        ConvolutionEvaluator = convolutionEvaluator;
        Visitor              = visitor;
    }

    public bool? Calculate( AstExpressionNode expr )
    {
        if( expr.ChildNodeCount != 2 )
        {
            throw new ArgumentException( $"Expected 2 child nodes, but got {expr.ChildNodeCount}. (node: {expr.GetType().Name})" );
        }

        var exprLeft = expr.Left.Accept( Visitor ) as AstExpressionNode;
        var exprRight = expr.Right.Accept( Visitor ) as AstExpressionNode;

        if( exprLeft == null || exprRight == null )
        {
            return null;
        }

        if( !exprLeft.TypeFlag.IsBoolean() || !exprRight.TypeFlag.IsBoolean() ||
            !exprLeft.Constant || !exprRight.Constant )
        {
            return null;
        }

        var convolutedLeft = ConvolutionEvaluator.Evaluate( exprLeft,   false );
        var convolutedRight = ConvolutionEvaluator.Evaluate( exprRight, false );

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
