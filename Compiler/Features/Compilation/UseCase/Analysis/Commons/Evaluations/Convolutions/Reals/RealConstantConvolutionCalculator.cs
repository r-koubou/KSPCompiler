using System;

using KSPCompiler.Features.Compilation.Domain.Ast.Nodes;
using KSPCompiler.Features.Compilation.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Features.Compilation.UseCase.Abstractions.Evaluations.Convolutions.Reals;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Commons.Evaluations.Convolutions.Reals;

/// <summary>
/// Calculator for convolution operations with KSP real (floating-point) operands
/// </summary>
public sealed class RealConstantConvolutionCalculator : IRealConstantConvolutionCalculator
{
    public double? Calculate( IAstVisitor visitor, AstExpressionNode expr, double _ )
    {
        if( expr.ChildNodeCount != 0 )
        {
            throw new ArgumentException( $"Expected 0 child nodes, but got {expr.ChildNodeCount}. (node: {expr.GetType().Name})" );
        }

        if( expr.Accept( visitor ) is not AstExpressionNode evaluatedNode )
        {
            throw new AstAnalyzeException( expr, "Failed to evaluate expression" );
        }

        if( evaluatedNode is AstRealLiteralNode literal )
        {
            return literal.Value;
        }

        return null;
    }
}
