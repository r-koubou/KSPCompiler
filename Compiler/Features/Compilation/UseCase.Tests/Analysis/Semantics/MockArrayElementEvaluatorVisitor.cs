using System;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Symbols;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions;

namespace KSPCompiler.Features.Compilation.UseCase.Tests.Analysis.Semantics;

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
