using KSPCompiler.Shared.Domain.Ast.Nodes;

namespace KSPCompiler.Features.Compilation.UseCase.Abstractions.Evaluations.Declarations;

public interface IDeclarationEvaluator<in TNode> where TNode : IAstNode
{
    IAstNode Evaluate( IAstVisitor visitor, TNode node );
}
