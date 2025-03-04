using KSPCompiler.Shared.Domain.Ast.Nodes;
using KSPCompiler.Shared.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Features.Compilation.UseCase.Abstractions.Evaluations.Commands;

public interface ICallCommandEvaluator
{
    public IAstNode Evaluate( IAstVisitor visitor, AstCallCommandExpressionNode node );
}
