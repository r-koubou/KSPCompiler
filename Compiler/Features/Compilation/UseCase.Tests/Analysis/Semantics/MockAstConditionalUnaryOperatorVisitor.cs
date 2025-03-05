using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Operators;
using KSPCompiler.Shared.Domain.Ast.Nodes;
using KSPCompiler.Shared.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Features.Compilation.UseCase.Tests.Analysis.Semantics;

public class MockAstConditionalUnaryOperatorVisitor : DefaultAstVisitor
{
    private IUnaryOperatorEvaluator ConditionalUnaryOperatorEvaluator { get; set; } = new MockConditionalUnaryOperatorEvaluator();

    public void Inject( IUnaryOperatorEvaluator evaluator )
        => ConditionalUnaryOperatorEvaluator = evaluator;

    public override IAstNode Visit( AstUnaryLogicalNotExpressionNode node )
        => ConditionalUnaryOperatorEvaluator.Evaluate( this, node );
}
