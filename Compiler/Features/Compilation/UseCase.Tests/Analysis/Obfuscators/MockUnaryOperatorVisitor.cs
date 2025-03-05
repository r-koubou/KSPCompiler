using System;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Operators;
using KSPCompiler.Shared.Domain.Ast.Nodes;
using KSPCompiler.Shared.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Features.Compilation.UseCase.Tests.Analysis.Obfuscators;

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
