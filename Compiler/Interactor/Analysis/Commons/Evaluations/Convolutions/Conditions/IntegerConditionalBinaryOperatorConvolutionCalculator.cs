using System;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;
using KSPCompiler.UseCases.Analysis.Evaluations.Convolutions.Conditions;
using KSPCompiler.UseCases.Analysis.Evaluations.Convolutions.Integers;

namespace KSPCompiler.Interactor.Analysis.Commons.Evaluations.Convolutions.Conditions;

/// <summary>
/// Calculator for convolution operations with conditional operators (integer)
/// </summary>
public sealed class IntegerConditionalBinaryOperatorConvolutionCalculator : IIntegerConditionalBinaryOperatorConvolutionCalculator
{
    private IIntegerConvolutionEvaluator ConvolutionEvaluator { get; }

    public IntegerConditionalBinaryOperatorConvolutionCalculator( IIntegerConvolutionEvaluator convolutionEvaluator )
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

        if( !exprLeft.TypeFlag.IsInt() || !exprRight.TypeFlag.IsInt() ||
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
            AstNodeId.Equal        => convolutedLeft == convolutedRight,
            AstNodeId.NotEqual     => convolutedLeft != convolutedRight,
            AstNodeId.GreaterThan  => convolutedLeft > convolutedRight,
            AstNodeId.GreaterEqual => convolutedLeft >= convolutedRight,
            AstNodeId.LessThan     => convolutedLeft < convolutedRight,
            AstNodeId.LessEqual    => convolutedLeft <= convolutedRight,
            _                      => null
        };
    }
}
