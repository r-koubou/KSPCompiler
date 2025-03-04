using KSPCompiler.Shared.Domain.Ast.Nodes;
using KSPCompiler.Shared.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Symbols;

public interface IArrayElementEvaluator
{
    public IAstNode Evaluate( IAstVisitor visitor, AstArrayElementExpressionNode expr );
}
