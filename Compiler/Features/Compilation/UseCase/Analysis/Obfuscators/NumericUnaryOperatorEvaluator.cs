using System;
using System.Text;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Interactors.Analysis.Obfuscators.Extensions;
using KSPCompiler.UseCases.Analysis.Evaluations.Operators;

namespace KSPCompiler.Interactors.Analysis.Obfuscators;

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
