using System;
using System.Text;

using KSPCompiler.Features.Compilation.Domain.Ast.Nodes;
using KSPCompiler.Features.Compilation.UseCase.Abstractions.Evaluations.Operators;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators.Extensions;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators;

public class ConditionalLogicalOperatorEvaluator : IConditionalLogicalOperatorEvaluator
{
    private StringBuilder OutputBuilder { get; }

    public ConditionalLogicalOperatorEvaluator( StringBuilder outputBuilder )
    {
        OutputBuilder = outputBuilder;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstExpressionNode expr )
    {
        _ = expr.Id switch
        {
            AstNodeId.LogicalOr  => OutputBuilder.AppendBinaryOperator( visitor, "or",  expr.Left, expr.Right ),
            AstNodeId.LogicalAnd => OutputBuilder.AppendBinaryOperator( visitor, "and", expr.Left, expr.Right ),
            AstNodeId.LogicalXor => OutputBuilder.AppendBinaryOperator( visitor, "xor", expr.Left, expr.Right ),
            _                    => throw new ArgumentException( $"Invalid logical operator: {expr.Id}" )
        };

        return expr;
    }
}
