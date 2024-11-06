using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.UseCases.Analysis.Evaluations.Commands;

namespace KSPCompiler.Interactors.Tests.Analysis.Semantics;

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
