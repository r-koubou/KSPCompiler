using System;

using KSPCompiler.Features.Compilation.UseCase.Abstractions.Evaluations.Convolutions.Booleans;
using KSPCompiler.Shared.Domain.Ast.Nodes;
using KSPCompiler.Shared.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Commons.Evaluations.Convolutions.Booleans;

/// <summary>
/// Calculator for convolution operations with boolean constants
/// </summary>
public class BooleanConstantConvolutionCalculator : IBooleanConstantConvolutionCalculator
{
    public bool? Calculate( IAstVisitor visitor, AstExpressionNode expr, bool workingValueForRecursive )
    {
        if( expr.ChildNodeCount != 0 )
        {
            throw new ArgumentException( $"Expected 0 child nodes, but got {expr.ChildNodeCount}. (node: {expr.GetType().Name})" );
        }

        if( expr is AstBooleanLiteralNode literal )
        {
            return literal.Value;
        }

        return null;
    }
}
