using System;
using System.Text;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Operators;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators.Extensions;
using KSPCompiler.Shared.Domain.Ast.Nodes;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators;

public class ConditionalBinaryOperatorEvaluator : IConditionalBinaryOperatorEvaluator
{
    private StringBuilder OutputBuilder { get; }

    public ConditionalBinaryOperatorEvaluator( StringBuilder outputBuilder )
    {
        OutputBuilder = outputBuilder;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstExpressionNode expr )
    {
        var _ = expr.Id switch
        {
            AstNodeId.Equal        => OutputBuilder.AppendBinaryOperator( visitor, "=",  expr.Left, expr.Right ),
            AstNodeId.NotEqual     => OutputBuilder.AppendBinaryOperator( visitor, "#",  expr.Left, expr.Right ),
            AstNodeId.LessThan     => OutputBuilder.AppendBinaryOperator( visitor, "<",  expr.Left, expr.Right ),
            AstNodeId.GreaterThan  => OutputBuilder.AppendBinaryOperator( visitor, ">",  expr.Left, expr.Right ),
            AstNodeId.LessEqual    => OutputBuilder.AppendBinaryOperator( visitor, "<=", expr.Left, expr.Right ),
            AstNodeId.GreaterEqual => OutputBuilder.AppendBinaryOperator( visitor, ">=", expr.Left, expr.Right ),
            _                      => throw new ArgumentException( $"Invalid binary operator: {expr.Id}" )
        };

        return expr;
    }
}
