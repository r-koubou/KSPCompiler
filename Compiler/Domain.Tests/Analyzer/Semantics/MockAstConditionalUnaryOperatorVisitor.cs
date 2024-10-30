using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Operators;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

public class MockAstConditionalUnaryOperatorVisitor : DefaultAstVisitor
{
    private IUnaryOperatorEvaluator ConditionalUnaryOperatorEvaluator { get; set; } = new MockConditionalUnaryOperatorEvaluator();

    public void Inject( IUnaryOperatorEvaluator evaluator )
        => ConditionalUnaryOperatorEvaluator = evaluator;

    public override IAstNode Visit( AstUnaryLogicalNotExpressionNode node )
        => ConditionalUnaryOperatorEvaluator.Evaluate( this, node );
}
