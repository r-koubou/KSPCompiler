using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Declarations;

public interface IDeclarationEvaluator<in TNode> where TNode : IAstNode
{
    IAstNode Evaluate( IAstVisitor visitor, TNode node );
}
