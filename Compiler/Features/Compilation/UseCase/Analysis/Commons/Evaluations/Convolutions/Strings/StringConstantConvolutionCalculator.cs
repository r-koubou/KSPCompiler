using System;
using System.Globalization;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Convolutions.Strings;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Commons.Evaluations.Convolutions.Strings;

/// <summary>
/// Calculator for convolution operations with string constants
/// </summary>
public sealed class StringConstantConvolutionCalculator : IStringConstantConvolutionCalculator
{
    public string? Calculate( IAstVisitor visitor, AstExpressionNode expr, string _ )
    {
        if( expr.ChildNodeCount != 0 )
        {
            throw new ArgumentException( $"Expected 0 child nodes, but got {expr.ChildNodeCount}. (node: {expr.GetType().Name})" );
        }

        if( expr.Accept( visitor ) is not AstExpressionNode evaluatedNode )
        {
            throw new AstAnalyzeException( expr, "Failed to evaluate expression" );
        }

        return evaluatedNode switch
        {
            AstStringLiteralNode literal   => literal.Value,
            AstIntLiteralNode intLiteral   => intLiteral.Value.ToString(),
            AstRealLiteralNode realLiteral => realLiteral.Value.ToString( CultureInfo.InvariantCulture ),
            _                              => null
        };
    }
}
