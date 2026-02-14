using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Commands;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Operators;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions;

namespace KSPCompiler.Features.SymbolManagement.UseCase.Tests.Analysis.Semantics;

public class MockCallCommandExpressionVisitor : DefaultAstVisitor
{
    private ICallCommandEvaluator CallCommandEvaluator { get; set; } = new MockICallCommandEvaluator();
    private IBinaryOperatorEvaluator? NumericBinaryOperatorEvaluator { get; set; }

    public void Inject( ICallCommandEvaluator iCallCommandEvaluator )
    {
        CallCommandEvaluator = iCallCommandEvaluator;
    }

    public void Inject( IBinaryOperatorEvaluator binaryOperatorEvaluator )
    {
        NumericBinaryOperatorEvaluator = binaryOperatorEvaluator;
    }

    public override IAstNode Visit( AstCallCommandExpressionNode node )
        => CallCommandEvaluator.Evaluate( this, node );

    public override IAstNode Visit( AstAdditionExpressionNode node )
        => NumericBinaryOperatorEvaluator?.Evaluate( this, node ) ?? base.Visit( node );

    public override IAstNode Visit( AstSubtractionExpressionNode node )
        => NumericBinaryOperatorEvaluator?.Evaluate( this, node ) ?? base.Visit( node );

    public override IAstNode Visit( AstMultiplyingExpressionNode node )
        => NumericBinaryOperatorEvaluator?.Evaluate( this, node ) ?? base.Visit( node );

    public override IAstNode Visit( AstDivisionExpressionNode node )
        => NumericBinaryOperatorEvaluator?.Evaluate( this, node ) ?? base.Visit( node );

    public override IAstNode Visit( AstModuloExpressionNode node )
        => NumericBinaryOperatorEvaluator?.Evaluate( this, node ) ?? base.Visit( node );

    public override IAstNode Visit( AstBitwiseOrExpressionNode node )
        => NumericBinaryOperatorEvaluator?.Evaluate( this, node ) ?? base.Visit( node );

    public override IAstNode Visit( AstBitwiseAndExpressionNode node )
        => NumericBinaryOperatorEvaluator?.Evaluate( this, node ) ?? base.Visit( node );

    public override IAstNode Visit( AstBitwiseXorExpressionNode node )
        => NumericBinaryOperatorEvaluator?.Evaluate( this, node ) ?? base.Visit( node );
}
