using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Operators;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

public class MockAstConditionalLogicalOperatorVisitor : DefaultAstVisitor
{
    private IConditionalLogicalOperatorEvaluator ConditionalBinaryOperatorEvaluator { get; set; } = new MockConditionalLogicalOperatorEvaluator();

    public void Inject( IConditionalLogicalOperatorEvaluator evaluator )
        => ConditionalBinaryOperatorEvaluator = evaluator;

    public override IAstNode Visit( AstLogicalOrExpressionNode node )
        => ConditionalBinaryOperatorEvaluator.Evaluate( this, node );

    public override IAstNode Visit( AstLogicalAndExpressionNode node )
        => ConditionalBinaryOperatorEvaluator.Evaluate( this, node );

    public override IAstNode Visit( AstLogicalXorExpressionNode node )
        => ConditionalBinaryOperatorEvaluator.Evaluate( this, node );
}
