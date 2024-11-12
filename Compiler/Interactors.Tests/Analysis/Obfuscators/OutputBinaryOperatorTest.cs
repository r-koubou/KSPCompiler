using System;
using System.Text;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Interactors.Analysis.Obfuscators.Extensions;
using KSPCompiler.UseCases.Analysis.Evaluations.Operators;

using NUnit.Framework;

namespace KSPCompiler.Interactors.Tests.Analysis.Obfuscators;

[TestFixture]
public class OutputBinaryOperatorTest
{
}

public class NumericBinaryOperatorEvaluator : IBinaryOperatorEvaluator
{
    private StringBuilder OutputBuilder { get; }

    public NumericBinaryOperatorEvaluator( StringBuilder outputBuilder )
    {
        OutputBuilder               = outputBuilder;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstExpressionNode expr )
    {
        _ = expr.Id switch
        {
            AstNodeId.Addition    => OutputBuilder.AppendBinaryOperator( visitor, "+", expr.Left, expr.Right ),
            AstNodeId.Subtraction => OutputBuilder.AppendBinaryOperator( visitor, "-", expr.Left, expr.Right ),
            AstNodeId.Multiplying => OutputBuilder.AppendBinaryOperator( visitor, "*", expr.Left, expr.Right ),
            AstNodeId.Division    => OutputBuilder.AppendBinaryOperator( visitor, "/", expr.Left, expr.Right ),
            AstNodeId.Modulo      => OutputBuilder.AppendBinaryOperator( visitor, "mod", expr.Left, expr.Right ),
            _                     => throw new ArgumentException( $"Invalid binary operator: {expr.Id}" )
        };

        return expr;
    }
}
