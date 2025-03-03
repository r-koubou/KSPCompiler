using KSPCompiler.Features.Compilation.Domain.Ast.Nodes;
using KSPCompiler.Features.Compilation.Domain.Ast.Nodes.Statements;

namespace KSPCompiler.Features.Compilation.UseCase.Abstractions.Evaluations.UserFunctions;

public interface ICallUserFunctionEvaluator
{
    IAstNode Evaluate( IAstVisitor visitor, AstCallUserFunctionStatementNode statement );
}
