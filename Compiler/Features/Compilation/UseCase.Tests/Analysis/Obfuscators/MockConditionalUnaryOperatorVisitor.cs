using System;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Operators;
using KSPCompiler.Shared.Domain.Ast.Nodes;
using KSPCompiler.Shared.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Features.Compilation.UseCase.Tests.Analysis.Obfuscators;

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
