using System;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;

namespace KSPCompiler.Domain.Ast.Analyzers.Convolutions.Reals;

/// <summary>
/// Calculator for convolution operations with conditional operators
/// </summary>
public sealed class RealConditionalOperatorConvolutionEvaluator : IPrimitiveConvolutionConditionalEvaluator<double>
{
    private IAstVisitor Visitor { get; }
    private IPrimitiveConvolutionEvaluator<double> ConvolutionEvaluator { get; }

    public RealConditionalOperatorConvolutionEvaluator(
        IAstVisitor visitor,
        IPrimitiveConvolutionEvaluator<double> convolutionEvaluator )
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

        var exprLeft = expr.Accept( Visitor,  abortTraverseToken ) as AstExpressionNode;
        var exprRight = expr.Accept( Visitor, abortTraverseToken ) as AstExpressionNode;

        if( exprLeft == null || exprRight == null )
        {
            return null;
        }

        if( !exprLeft.TypeFlag.IsReal() || !exprRight.TypeFlag.IsReal() ||
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
