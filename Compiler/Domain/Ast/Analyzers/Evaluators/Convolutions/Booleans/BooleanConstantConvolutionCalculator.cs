using System;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Booleans;

/// <summary>
/// Calculator for convolution operations with boolean constants
/// </summary>
public class BooleanConstantConvolutionCalculator : IBooleanConstantConvolutionCalculator
{
    public bool? Calculate( AstExpressionNode expr, bool workingValueForRecursive )
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
