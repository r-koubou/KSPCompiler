using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.UseCases.Analysis.Evaluations.Operators;

namespace KSPCompiler.Interactors.Tests.Analysis.Semantics;

public class MockAstConditionalUnaryOperatorVisitor : DefaultAstVisitor
{
    private IUnaryOperatorEvaluator ConditionalUnaryOperatorEvaluator { get; set; } = new MockConditionalUnaryOperatorEvaluator();

    public void Inject( IUnaryOperatorEvaluator evaluator )
        => ConditionalUnaryOperatorEvaluator = evaluator;

    public override IAstNode Visit( AstUnaryLogicalNotExpressionNode node )
        => ConditionalUnaryOperatorEvaluator.Evaluate( this, node );
}
