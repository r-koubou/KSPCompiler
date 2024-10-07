using System;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;

namespace KSPCompiler.Domain.Ast.Analyzers.Convolutions;

/// <summary>
/// Calculator for convolution operations with conditional operations
/// </summary>
public sealed class RealConvolutionConditionalEvaluator : IConvolutionConditionalEvaluator<double>
{
    private IAstVisitor<IAstNode> Visitor { get; }
    private AbortTraverseToken AbortTraverseToken { get; }
    private IConvolutionEvaluator<double> ConvolutionEvaluator { get; }

    public RealConvolutionConditionalEvaluator(
        IAstVisitor<IAstNode> visitor,
        AbortTraverseToken abortTraverseToken,
        IConvolutionEvaluator<double> convolutionEvaluator )
    {
        Visitor              = visitor;
        AbortTraverseToken   = abortTraverseToken;
        ConvolutionEvaluator = convolutionEvaluator;
    }

    public bool? Evaluate( AstExpressionSyntaxNode expr )
    {
        if( expr.ChildNodeCount != 2 )
        {
            throw new ArgumentException( $"Expected 2 child nodes, but got {expr.ChildNodeCount}. (node: {expr.GetType().Name})" );
        }

        if( AbortTraverseToken.Aborted )
        {
            return null;
        }

        var exprLeft = expr.Accept( Visitor, AbortTraverseToken ) as AstExpressionSyntaxNode;
        var exprRight = expr.Accept( Visitor, AbortTraverseToken ) as AstExpressionSyntaxNode;

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
