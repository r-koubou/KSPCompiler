using System;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.UseCases.Analysis.Evaluations.Convolutions.Booleans;

namespace KSPCompiler.Interactors.Analysis.Commons.Evaluations.Convolutions.Booleans;

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
