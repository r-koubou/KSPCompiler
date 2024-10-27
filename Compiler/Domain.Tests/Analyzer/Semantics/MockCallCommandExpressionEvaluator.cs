using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Commands;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

internal class MockCallCommandExpressionEvaluator : ICallCommandExpressionEvaluator
{
    public IAstNode Evaluate( IAstVisitor<IAstNode> visitor, AstCallCommandExpressionNode node )
        => throw new System.NotImplementedException();
}
