using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Statements;

namespace KSPCompiler.UseCases.Analysis.Evaluations.UserFunctions;

public interface ICallUserFunctionEvaluator
{
    IAstNode Evaluate( IAstVisitor visitor, AstCallUserFunctionStatementNode statement );
}
