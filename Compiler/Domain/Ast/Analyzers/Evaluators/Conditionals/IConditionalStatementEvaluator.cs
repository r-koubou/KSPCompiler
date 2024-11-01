using KSPCompiler.Domain.Ast.Nodes;

namespace KSPCompiler.Domain.Ast.Analyzers.Evaluators.Conditionals;

public interface IConditionalStatementEvaluator<in TStatementNode> where TStatementNode : AstStatementNode
{
    IAstNode Evaluate( IAstVisitor visitor, TStatementNode statement );
}
