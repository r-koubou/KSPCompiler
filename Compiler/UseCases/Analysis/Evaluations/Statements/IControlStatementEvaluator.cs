using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Statements;

namespace KSPCompiler.UseCases.Analysis.Evaluations.Statements;

public interface IControlStatementEvaluator
{
    public IAstNode Evaluate( IAstVisitor visitor, AstExitStatementNode node );
}
