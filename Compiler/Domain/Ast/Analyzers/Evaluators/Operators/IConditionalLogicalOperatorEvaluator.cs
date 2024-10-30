using KSPCompiler.Domain.Ast.Nodes;

namespace KSPCompiler.Domain.Ast.Analyzers.Evaluators.Operators;

public interface IConditionalLogicalOperatorEvaluator<TEvalResult>
{
    public TEvalResult Evaluate( IAstVisitor<TEvalResult> visitor, AstExpressionNode expr );
}

public interface IConditionalLogicalOperatorEvaluator : IConditionalLogicalOperatorEvaluator<IAstNode> {}
