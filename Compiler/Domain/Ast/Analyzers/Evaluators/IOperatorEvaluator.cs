using KSPCompiler.Domain.Ast.Nodes;

namespace KSPCompiler.Domain.Ast.Analyzers.Evaluators;

public interface IOperatorEvaluator<TEvalResult>
{
    public TEvalResult Evaluate( IAstVisitor<TEvalResult> visitor, AstExpressionNode expr, AbortTraverseToken abortTraverseToken );
}
