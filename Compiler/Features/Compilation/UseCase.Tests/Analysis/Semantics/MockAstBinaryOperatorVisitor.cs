using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Operators;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions;

namespace KSPCompiler.Features.Compilation.UseCase.Tests.Analysis.Semantics;

public class MockAstBinaryOperatorVisitor : DefaultAstVisitor
{
    private IBinaryOperatorEvaluator NumericBinaryOperatorEvaluator { get; set; } = new MockBinaryOperatorEvaluator();

    public void Inject( IBinaryOperatorEvaluator binaryOperatorEvaluator )
    {
        NumericBinaryOperatorEvaluator = binaryOperatorEvaluator;
    }

    #region Binary Operators (Mathematical)

    public override IAstNode Visit( AstAdditionExpressionNode node )
        => NumericBinaryOperatorEvaluator.Evaluate( this, node );

    public override IAstNode Visit( AstSubtractionExpressionNode node )
        => NumericBinaryOperatorEvaluator.Evaluate( this, node );

    public override IAstNode Visit( AstMultiplyingExpressionNode node )
        => NumericBinaryOperatorEvaluator.Evaluate( this, node );

    public override IAstNode Visit( AstDivisionExpressionNode node )
        => NumericBinaryOperatorEvaluator.Evaluate( this, node );

    public override IAstNode Visit( AstModuloExpressionNode node )
        => NumericBinaryOperatorEvaluator.Evaluate( this, node );

    #endregion ~Binary Operators

    #region Binary Operators (Bitwise)
    public override IAstNode Visit( AstBitwiseOrExpressionNode node )
        => NumericBinaryOperatorEvaluator.Evaluate( this, node );

    public override IAstNode Visit( AstBitwiseAndExpressionNode node )
        => NumericBinaryOperatorEvaluator.Evaluate( this, node );

    public override IAstNode Visit( AstBitwiseXorExpressionNode node )
        => NumericBinaryOperatorEvaluator.Evaluate( this, node );

    #endregion ~Binary Operators (Bitwise)
}
