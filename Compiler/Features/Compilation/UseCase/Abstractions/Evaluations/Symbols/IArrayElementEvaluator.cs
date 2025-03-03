using KSPCompiler.Features.Compilation.Domain.Ast.Nodes;
using KSPCompiler.Features.Compilation.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Features.Compilation.UseCase.Abstractions.Evaluations.Symbols;

public interface IArrayElementEvaluator
{
    public IAstNode Evaluate( IAstVisitor visitor, AstArrayElementExpressionNode expr );
}
