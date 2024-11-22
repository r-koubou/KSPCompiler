using System;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.UseCases.Analysis.Evaluations.Symbols;

namespace KSPCompiler.Interactors.Tests.Analysis.Semantics;

public class MockArrayElementEvaluatorVisitor : DefaultAstVisitor
{
    private IArrayElementEvaluator Evaluator { get; set; } = new MockArrayElementEvaluator();

    public void Inject( IArrayElementEvaluator evaluator )
        => Evaluator = evaluator;

    public override IAstNode Visit( AstArrayElementExpressionNode node )
        => Evaluator.Evaluate( this, node );

    private class MockArrayElementEvaluator : IArrayElementEvaluator
    {
        public IAstNode Evaluate( IAstVisitor visitor, AstArrayElementExpressionNode expr )
            => throw new NotImplementedException();
    }
}
