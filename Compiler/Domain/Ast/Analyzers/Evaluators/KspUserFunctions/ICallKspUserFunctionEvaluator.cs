using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Statements;

namespace KSPCompiler.Domain.Ast.Analyzers.Evaluators.KspUserFunctions;

public interface ICallKspUserFunctionEvaluator
{
    IAstNode Evaluate( IAstVisitor visitor, AstCallKspUserFunctionStatementNode statement );
}
