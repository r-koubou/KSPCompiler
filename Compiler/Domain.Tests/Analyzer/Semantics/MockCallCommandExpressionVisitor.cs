using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Commands;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

public class MockCallCommandExpressionVisitor : DefaultAstVisitor
{
    private ICallCommandExpressionEvaluator CallCommandExpressionEvaluator { get; set; } = new MockCallCommandExpressionEvaluator();

    public void Inject( ICallCommandExpressionEvaluator callCommandExpressionEvaluator )
    {
        CallCommandExpressionEvaluator = callCommandExpressionEvaluator;
    }

    public override IAstNode Visit( AstCallCommandExpressionNode node )
        => CallCommandExpressionEvaluator.Evaluate( this, node );
}
