using KSPCompiler.Domain.Ast.Nodes;

namespace KSPCompiler.UseCases.Analysis.Evaluations.Declarations;

public interface IDeclarationEvaluator<in TNode> where TNode : IAstNode
{
    IAstNode Evaluate( IAstVisitor visitor, TNode node );
}
