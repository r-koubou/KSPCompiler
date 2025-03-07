using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Operators;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions;

namespace KSPCompiler.Features.Compilation.UseCase.Tests.Analysis.Semantics;

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
