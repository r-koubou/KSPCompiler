using System;
using System.Text;

using KSPCompiler.Features.Compilation.Domain.Ast.Nodes;
using KSPCompiler.Features.Compilation.UseCase.Abstractions.Evaluations.Operators;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators.Extensions;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators;

public class NumericUnaryOperatorEvaluator : IUnaryOperatorEvaluator
{
    private StringBuilder OutputBuilder { get; }

    public NumericUnaryOperatorEvaluator( StringBuilder outputBuilder )
    {
        OutputBuilder = outputBuilder;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstExpressionNode expr )
    {
        _ = expr.Id switch
        {
            AstNodeId.UnaryMinus => OutputBuilder.AppendUnaryOperator( visitor, "-",     expr.Left ),
            AstNodeId.UnaryNot   => OutputBuilder.AppendUnaryOperator( visitor, ".not.", expr.Left ),
            _                    => throw new ArgumentException( $"Invalid binary operator: {expr.Id}" )
        };

        return expr;
    }

}
