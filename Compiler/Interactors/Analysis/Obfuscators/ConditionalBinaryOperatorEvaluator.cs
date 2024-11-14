using System;
using System.Text;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Interactors.Analysis.Obfuscators.Extensions;
using KSPCompiler.UseCases.Analysis.Evaluations.Operators;

namespace KSPCompiler.Interactors.Analysis.Obfuscators;

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
