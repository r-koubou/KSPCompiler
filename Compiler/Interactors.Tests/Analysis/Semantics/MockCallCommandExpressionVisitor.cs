using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.UseCases.Analysis.Evaluations.Commands;

namespace KSPCompiler.Interactors.Tests.Analysis.Semantics;

public class MockCallCommandExpressionVisitor : DefaultAstVisitor
{
    private ICallCommandEvaluator CallCommandEvaluator { get; set; } = new MockICallCommandEvaluator();

    public void Inject( ICallCommandEvaluator iCallCommandEvaluator )
    {
        CallCommandEvaluator = iCallCommandEvaluator;
    }

    public override IAstNode Visit( AstCallCommandExpressionNode node )
        => CallCommandEvaluator.Evaluate( this, node );
}
