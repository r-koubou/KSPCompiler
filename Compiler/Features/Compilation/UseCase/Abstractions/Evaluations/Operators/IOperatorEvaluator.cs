using KSPCompiler.Shared.Domain.Ast.Nodes;

namespace KSPCompiler.Features.Compilation.UseCase.Abstractions.Evaluations.Operators;

public interface IOperatorEvaluator
{
    public IAstNode Evaluate( IAstVisitor visitor, AstExpressionNode expr );
}
