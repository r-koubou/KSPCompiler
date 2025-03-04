using System;
using System.Text;

using KSPCompiler.Features.Compilation.UseCase.Abstractions.Evaluations.Operators;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators.Extensions;
using KSPCompiler.Shared.Domain.Ast.Nodes;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators;

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
