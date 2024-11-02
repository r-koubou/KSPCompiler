using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Domain.Ast.Analyzers.Evaluators.Assigns;

public interface IAssignStatementEvaluator<in TNode> where TNode : AstStatementNode
{
    public IAstNode Evaluate( IAstVisitor visitor, TNode statement );
}

public interface IAssignStatementEvaluator : IAssignStatementEvaluator<AstAssignStatementNode> {}
