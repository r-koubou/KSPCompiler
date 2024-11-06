using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.UseCases.Analysis.Evaluations.Operators;

namespace KSPCompiler.Interactor.Tests.Analysis.Semantics;

public class MockAstConditionalBinaryOperatorVisitor : DefaultAstVisitor
{
    private IBinaryOperatorEvaluator ConditionalBinaryOperatorEvaluator { get; set; } = new MockConditionalBinaryOperatorEvaluator();

    public void Inject( IBinaryOperatorEvaluator evaluator )
        => ConditionalBinaryOperatorEvaluator = evaluator;

    public override IAstNode Visit( AstEqualExpressionNode node )
        => ConditionalBinaryOperatorEvaluator.Evaluate( this, node );

    public override IAstNode Visit( AstNotEqualExpressionNode node )
        => ConditionalBinaryOperatorEvaluator.Evaluate( this, node );

    public override IAstNode Visit( AstLessThanExpressionNode node )
        => ConditionalBinaryOperatorEvaluator.Evaluate( this, node );

    public override IAstNode Visit( AstGreaterThanExpressionNode node )
        => ConditionalBinaryOperatorEvaluator.Evaluate( this, node );

    public override IAstNode Visit( AstLessEqualExpressionNode node )
        => ConditionalBinaryOperatorEvaluator.Evaluate( this, node );

    public override IAstNode Visit( AstGreaterEqualExpressionNode node )
        => ConditionalBinaryOperatorEvaluator.Evaluate( this, node );
}
