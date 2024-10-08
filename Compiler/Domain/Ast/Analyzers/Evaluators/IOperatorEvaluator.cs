using KSPCompiler.Domain.Ast.Nodes;

namespace KSPCompiler.Domain.Ast.Analyzers.Evaluators;

public interface IOperatorEvaluator<TEvalResult>
{
    public TEvalResult Evaluate( IAstVisitor<TEvalResult> visitor, AstExpressionSyntaxNode expr, AbortTraverseToken abortTraverseToken );
}

public interface IOperatorEvaluator : IOperatorEvaluator<IAstNode> {}