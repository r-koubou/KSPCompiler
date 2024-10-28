using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Domain.Ast.Analyzers.Evaluators.Commands;

public interface ICallCommandExpressionEvaluator<TEvalResult>
{
    public TEvalResult Evaluate( IAstVisitor<TEvalResult> visitor, AstCallCommandExpressionNode node );
}

public interface ICallCommandExpressionEvaluator : ICallCommandExpressionEvaluator<IAstNode> {}
