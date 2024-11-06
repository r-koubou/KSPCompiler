using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.UseCases.Analysis.Evaluations.Operators;

namespace KSPCompiler.Interactors.Tests.Analysis.Semantics;

public class MockAstUnaryOperatorVisitor : DefaultAstVisitor
{
    private IUnaryOperatorEvaluator NumericUnaryOperatorEvaluator { get; set; } = new MockUnaryOperatorEvaluator();

    public void Inject( IUnaryOperatorEvaluator unaryOperatorEvaluator )
    {
        NumericUnaryOperatorEvaluator = unaryOperatorEvaluator;
    }

    public override IAstNode Visit( AstUnaryNotExpressionNode node )
        => NumericUnaryOperatorEvaluator.Evaluate( this, node );

    public override IAstNode Visit( AstUnaryMinusExpressionNode node )
        => NumericUnaryOperatorEvaluator.Evaluate( this, node );
}
