using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Operators;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

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
