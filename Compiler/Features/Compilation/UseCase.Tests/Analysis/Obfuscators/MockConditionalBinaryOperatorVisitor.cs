using System;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Operators;
using KSPCompiler.Shared.Domain.Ast.Nodes;
using KSPCompiler.Shared.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Features.Compilation.UseCase.Tests.Analysis.Obfuscators;

public class MockConditionalBinaryOperatorVisitor : DefaultAstVisitor
{
    private IConditionalBinaryOperatorEvaluator Evaluator { get; set; } = new MockConditionalBinaryOperatorEvaluator();

    public void Inject( IConditionalBinaryOperatorEvaluator evaluator )
    {
        Evaluator = evaluator;
    }

    public override IAstNode Visit( AstEqualExpressionNode node )
        => Evaluator.Evaluate( this, node );

    public override IAstNode Visit( AstNotEqualExpressionNode node )
        => Evaluator.Evaluate( this, node );

    public override IAstNode Visit( AstLessThanExpressionNode node )
        => Evaluator.Evaluate( this, node );

    public override IAstNode Visit( AstGreaterThanExpressionNode node )
        => Evaluator.Evaluate( this, node );

    public override IAstNode Visit( AstLessEqualExpressionNode node )
        => Evaluator.Evaluate( this, node );

    public override IAstNode Visit( AstGreaterEqualExpressionNode node )
        => Evaluator.Evaluate( this, node );

    private class MockConditionalBinaryOperatorEvaluator : IConditionalBinaryOperatorEvaluator
    {
        public IAstNode Evaluate( IAstVisitor visitor, AstExpressionNode expr )
            => throw new NotImplementedException();
    }
}
