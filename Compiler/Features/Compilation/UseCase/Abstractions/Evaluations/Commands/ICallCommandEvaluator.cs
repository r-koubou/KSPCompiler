using KSPCompiler.Features.Compilation.Domain.Ast.Nodes;
using KSPCompiler.Features.Compilation.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Features.Compilation.UseCase.Abstractions.Evaluations.Commands;

public interface ICallCommandEvaluator
{
    public IAstNode Evaluate( IAstVisitor visitor, AstCallCommandExpressionNode node );
}
