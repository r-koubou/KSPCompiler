using System;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;

namespace KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Integers;

/// <summary>
/// Calculator for convolution operations with conditional operators (integer)
/// </summary>
public sealed class IntegerConditionalOperatorConvolutionEvaluator : IIntegerConditionalOperatorConvolutionEvaluator
{
    private IAstVisitor Visitor { get; }
    private IIntegerConvolutionEvaluator ConvolutionEvaluator { get; }

    public IntegerConditionalOperatorConvolutionEvaluator(
        IAstVisitor visitor,
        IIntegerConvolutionEvaluator convolutionEvaluator )
    {
        Visitor              = visitor;
        ConvolutionEvaluator = convolutionEvaluator;
    }

    public bool? Evaluate( AstExpressionNode expr, AbortTraverseToken abortTraverseToken )
    {
        if( expr.ChildNodeCount != 2 )
        {
            throw new ArgumentException( $"Expected 2 child nodes, but got {expr.ChildNodeCount}. (node: {expr.GetType().Name})" );
        }

        if( abortTraverseToken.Aborted )
        {
            return null;
        }

        var exprLeft = expr.Accept( Visitor, abortTraverseToken ) as AstExpressionNode;
        var exprRight = expr.Accept( Visitor, abortTraverseToken ) as AstExpressionNode;

        if( exprLeft == null || exprRight == null )
        {
            return null;
        }

        if( !exprLeft.TypeFlag.IsInt() || !exprRight.TypeFlag.IsInt() ||
            !exprLeft.IsConstant || !exprRight.IsConstant )
        {
            return null;
        }

        var convolutedLeft = ConvolutionEvaluator.Evaluate( exprLeft,   0 );
        var convolutedRight = ConvolutionEvaluator.Evaluate( exprRight, 0 );

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
