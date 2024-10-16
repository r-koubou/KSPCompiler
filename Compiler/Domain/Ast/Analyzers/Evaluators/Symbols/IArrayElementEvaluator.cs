using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Domain.Ast.Analyzers.Evaluators.Symbols;

public interface IArrayElementEvaluator<TEvalResult>
{
    public TEvalResult Evaluate( IAstVisitor<TEvalResult> visitor, AstArrayElementExpressionNode expr );
}

public interface IArrayElementEvaluator : IArrayElementEvaluator<IAstNode> {}
