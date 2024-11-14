using System;
using System.Text;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Interactors.Analysis.Obfuscators.Extensions;
using KSPCompiler.UseCases.Analysis.Evaluations.Operators;

namespace KSPCompiler.Interactors.Analysis.Obfuscators;

public class ConditionalUnaryOperatorEvaluator : IConditionalUnaryOperatorEvaluator
{
    private StringBuilder OutputBuilder { get; }

    public ConditionalUnaryOperatorEvaluator( StringBuilder outputBuilder )
    {
        OutputBuilder = outputBuilder;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstExpressionNode expr )
    {
        _ = expr.Id switch
        {
            AstNodeId.UnaryLogicalNot => OutputBuilder.AppendUnaryOperator( visitor, "not", expr.Left ),
            _                         => throw new ArgumentException( $"Invalid unary operator: {expr.Id}" )
        };

        return expr;
    }
}
