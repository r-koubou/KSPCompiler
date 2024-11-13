using System;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.UseCases.Analysis.Evaluations.Operators;

namespace KSPCompiler.Interactors.Tests.Analysis.Obfuscators;

public class MockConditionalLogicalOperatorVisitor : DefaultAstVisitor
{
    private IConditionalLogicalOperatorEvaluator Evaluator { get; set; } = new MockConditionalLogicalOperatorEvaluator();

    public void Inject( IConditionalLogicalOperatorEvaluator evaluator )
    {
        Evaluator = evaluator;
    }

    public override IAstNode Visit( AstLogicalOrExpressionNode node )
        => Evaluator.Evaluate( this, node );

    public override IAstNode Visit( AstLogicalAndExpressionNode node )
        => Evaluator.Evaluate( this, node );

    public override IAstNode Visit( AstLogicalXorExpressionNode node )
        => Evaluator.Evaluate( this, node );

    private class MockConditionalLogicalOperatorEvaluator : IConditionalLogicalOperatorEvaluator
    {
        public IAstNode Evaluate( IAstVisitor visitor, AstExpressionNode expr )
            => throw new NotImplementedException();
    }
}
