using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Operators;
using KSPCompiler.Shared.Domain.Ast.Nodes;
using KSPCompiler.Shared.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Features.Compilation.UseCase.Tests.Analysis.Semantics;

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
