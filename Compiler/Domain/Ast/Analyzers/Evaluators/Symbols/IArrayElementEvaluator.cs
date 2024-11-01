using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Domain.Ast.Analyzers.Evaluators.Symbols;

public interface IArrayElementEvaluator
{
    public IAstNode Evaluate( IAstVisitor visitor, AstArrayElementExpressionNode expr );
}
