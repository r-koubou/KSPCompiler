using KSPCompiler.Shared.Domain.Ast.Nodes;
using KSPCompiler.Shared.Domain.Ast.Nodes.Statements;

namespace KSPCompiler.Features.Compilation.UseCase.Abstractions.Evaluations.UserFunctions;

public interface ICallUserFunctionEvaluator
{
    IAstNode Evaluate( IAstVisitor visitor, AstCallUserFunctionStatementNode statement );
}
