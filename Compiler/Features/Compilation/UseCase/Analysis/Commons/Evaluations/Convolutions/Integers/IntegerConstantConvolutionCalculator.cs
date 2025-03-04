using System;

using KSPCompiler.Features.Compilation.UseCase.Abstractions.Evaluations.Convolutions.Integers;
using KSPCompiler.Shared.Domain.Ast.Nodes;
using KSPCompiler.Shared.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Commons.Evaluations.Convolutions.Integers;

/// <summary>
/// Calculator for convolution operations with integer constants
/// </summary>
public sealed class IntegerConstantConvolutionCalculator : IIntegerConstantConvolutionCalculator
{
    public int? Calculate( IAstVisitor visitor, AstExpressionNode expr, int workingValueForRecursive )
    {
        if( expr.ChildNodeCount != 0 )
        {
            throw new ArgumentException( $"Expected 0 child nodes, but got {expr.ChildNodeCount}. (node: {expr.GetType().Name})" );
        }

        if( expr.Accept( visitor ) is not AstExpressionNode evaluatedNode )
        {
            throw new AstAnalyzeException( expr, "Failed to evaluate expression" );
        }

        if( evaluatedNode is AstIntLiteralNode literal )
        {
            return literal.Value;
        }

        return null;
    }
}
