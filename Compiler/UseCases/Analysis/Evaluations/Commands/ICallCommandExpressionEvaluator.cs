using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.UseCases.Analysis.Evaluations.Commands;

public interface ICallCommandExpressionEvaluator
{
    public IAstNode Evaluate( IAstVisitor visitor, AstCallCommandExpressionNode node );
}
