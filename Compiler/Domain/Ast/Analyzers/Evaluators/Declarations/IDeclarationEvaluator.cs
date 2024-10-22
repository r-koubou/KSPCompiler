using KSPCompiler.Domain.Ast.Nodes;

namespace KSPCompiler.Domain.Ast.Analyzers.Evaluators.Declarations;

public interface IDeclarationEvaluator<in TNode> where TNode : IAstNode
{
    IAstNode Evaluate( IAstVisitor visitor, TNode node );
}
