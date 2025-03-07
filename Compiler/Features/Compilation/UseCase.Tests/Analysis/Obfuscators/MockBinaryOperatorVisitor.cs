using System;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Operators;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions;

namespace KSPCompiler.Features.Compilation.UseCase.Tests.Analysis.Obfuscators;

public class MockBinaryOperatorVisitor : DefaultAstVisitor
{
    private IOperatorEvaluator Evaluator { get; set; } = new MockBinaryOperatorEvaluator();

    public void Inject( IOperatorEvaluator evaluator )
    {
        Evaluator = evaluator;
    }

    public override IAstNode Visit( AstAdditionExpressionNode node )
        => Evaluator.Evaluate( this, node );

    public override IAstNode Visit( AstSubtractionExpressionNode node )
        => Evaluator.Evaluate( this, node );

    public override IAstNode Visit( AstMultiplyingExpressionNode node )
        => Evaluator.Evaluate( this, node );

    public override IAstNode Visit( AstDivisionExpressionNode node )
        => Evaluator.Evaluate( this, node );

    public override IAstNode Visit( AstModuloExpressionNode node )
        => Evaluator.Evaluate( this, node );

    public override IAstNode Visit( AstBitwiseOrExpressionNode node )
        => Evaluator.Evaluate( this, node );

    public override IAstNode Visit( AstBitwiseAndExpressionNode node )
        => Evaluator.Evaluate( this, node );

    public override IAstNode Visit( AstBitwiseXorExpressionNode node )
        => Evaluator.Evaluate( this, node );

    private class MockBinaryOperatorEvaluator : IBinaryOperatorEvaluator
    {
        public IAstNode Evaluate( IAstVisitor visitor, AstExpressionNode expr )
            => throw new NotImplementedException();
    }
}
