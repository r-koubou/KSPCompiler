using System;
using System.Text;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Interactors.Analysis.Obfuscators.Extensions;
using KSPCompiler.UseCases.Analysis.Evaluations.Operators;

namespace KSPCompiler.Interactors.Analysis.Obfuscators;

public class NumericBinaryOperatorEvaluator : IBinaryOperatorEvaluator
{
    private StringBuilder OutputBuilder { get; }

    public NumericBinaryOperatorEvaluator( StringBuilder outputBuilder )
    {
        OutputBuilder = outputBuilder;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstExpressionNode expr )
    {
        _ = expr.Id switch
        {
            AstNodeId.Addition    => OutputBuilder.AppendBinaryOperator( visitor, "+",     expr.Left, expr.Right ),
            AstNodeId.Subtraction => OutputBuilder.AppendBinaryOperator( visitor, "-",     expr.Left, expr.Right ),
            AstNodeId.Multiplying => OutputBuilder.AppendBinaryOperator( visitor, "*",     expr.Left, expr.Right ),
            AstNodeId.Division    => OutputBuilder.AppendBinaryOperator( visitor, "/",     expr.Left, expr.Right ),
            AstNodeId.Modulo      => OutputBuilder.AppendBinaryOperator( visitor, "mod",   expr.Left, expr.Right ),
            AstNodeId.BitwiseOr   => OutputBuilder.AppendBinaryOperator( visitor, ".or.",  expr.Left, expr.Right ),
            AstNodeId.BitwiseAnd  => OutputBuilder.AppendBinaryOperator( visitor, ".and.", expr.Left, expr.Right ),
            AstNodeId.BitwiseXor  => OutputBuilder.AppendBinaryOperator( visitor, ".xor.", expr.Left, expr.Right ),
            _                     => throw new ArgumentException( $"Invalid binary operator: {expr.Id}" )
        };

        return expr;
    }
}
