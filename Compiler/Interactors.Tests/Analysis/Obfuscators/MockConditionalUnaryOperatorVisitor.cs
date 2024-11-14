using System;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.UseCases.Analysis.Evaluations.Operators;

namespace KSPCompiler.Interactors.Tests.Analysis.Obfuscators;

public class MockConditionalUnaryOperatorVisitor : DefaultAstVisitor
{
    private IConditionalUnaryOperatorEvaluator Evaluator { get; set; } = new MockConditionalUnaryOperatorEvaluator();

    public void Inject( IConditionalUnaryOperatorEvaluator evaluator )
        => Evaluator = evaluator;

    public override IAstNode Visit( AstUnaryLogicalNotExpressionNode node )
        => Evaluator.Evaluate( this, node );

    private class MockConditionalUnaryOperatorEvaluator : IConditionalUnaryOperatorEvaluator
    {
        public IAstNode Evaluate( IAstVisitor visitor, AstExpressionNode expr )
            => throw new NotImplementedException();
    }
}
