using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Statements;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.UserFunctions;

public interface ICallUserFunctionEvaluator
{
    IAstNode Evaluate( IAstVisitor visitor, AstCallUserFunctionStatementNode statement );
}
