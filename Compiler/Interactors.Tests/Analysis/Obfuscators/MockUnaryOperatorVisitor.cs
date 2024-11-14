using System;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.UseCases.Analysis.Evaluations.Operators;

namespace KSPCompiler.Interactors.Tests.Analysis.Obfuscators;

public class MockUnaryOperatorVisitor : DefaultAstVisitor
{
    private IOperatorEvaluator Evaluator { get; set; } = new MockUnaryOperatorEvaluator();

    public void Inject( IOperatorEvaluator evaluator )
    {
        Evaluator = evaluator;
    }

    public override IAstNode Visit( AstUnaryMinusExpressionNode node )
        => Evaluator.Evaluate( this, node );

    public override IAstNode Visit( AstUnaryNotExpressionNode node )
        => Evaluator.Evaluate( this, node );

    private class MockUnaryOperatorEvaluator : IUnaryOperatorEvaluator
    {
        public IAstNode Evaluate( IAstVisitor visitor, AstExpressionNode expr )
            => throw new NotImplementedException();
    }
}
