using KSPCompiler.Domain.Ast.Nodes;

namespace KSPCompiler.UseCases.Analysis.Evaluations.Statements;

public interface IConditionalStatementEvaluator<in TStatementNode> where TStatementNode : AstStatementNode
{
    IAstNode Evaluate( IAstVisitor visitor, TStatementNode statement );
}
