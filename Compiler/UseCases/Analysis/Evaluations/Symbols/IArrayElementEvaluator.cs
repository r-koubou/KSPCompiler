using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.UseCases.Analysis.Evaluations.Symbols;

public interface IArrayElementEvaluator
{
    public IAstNode Evaluate( IAstVisitor visitor, AstArrayElementExpressionNode expr );
}
