using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.UseCases.Analysis.Evaluations.Operators;

namespace KSPCompiler.Interactor.Tests.Analysis.Semantics;

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
