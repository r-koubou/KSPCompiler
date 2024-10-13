using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Operators;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

public class MockAstBinaryOperatorVisitor : DefaultAstVisitor
{
    private IBinaryOperatorEvaluator NumericBinaryOperatorEvaluator { get; set; } = new MockBinaryOperatorEvaluator();

    public void Inject( IBinaryOperatorEvaluator binaryOperatorEvaluator )
    {
        NumericBinaryOperatorEvaluator = binaryOperatorEvaluator;
    }

    #region Binary Operators (Mathematical)

    public override IAstNode Visit( AstAdditionExpressionNode node, AbortTraverseToken abortTraverseToken )
        => NumericBinaryOperatorEvaluator.Evaluate( this, node, abortTraverseToken );

    public override IAstNode Visit( AstSubtractionExpressionNode node, AbortTraverseToken abortTraverseToken )
        => NumericBinaryOperatorEvaluator.Evaluate( this, node, abortTraverseToken );

    public override IAstNode Visit( AstMultiplyingExpressionNode node, AbortTraverseToken abortTraverseToken )
        => NumericBinaryOperatorEvaluator.Evaluate( this, node, abortTraverseToken );

    public override IAstNode Visit( AstDivisionExpressionNode node, AbortTraverseToken abortTraverseToken )
        => NumericBinaryOperatorEvaluator.Evaluate( this, node, abortTraverseToken );

    public override IAstNode Visit( AstModuloExpressionNode node, AbortTraverseToken abortTraverseToken )
        => NumericBinaryOperatorEvaluator.Evaluate( this, node, abortTraverseToken );

    #endregion ~Binary Operators

    #region Binary Operators (Bitwise)
    public override IAstNode Visit( AstBitwiseOrExpressionNode node, AbortTraverseToken abortTraverseToken )
        => NumericBinaryOperatorEvaluator.Evaluate( this, node, abortTraverseToken );

    public override IAstNode Visit( AstBitwiseAndExpressionNode node, AbortTraverseToken abortTraverseToken )
        => NumericBinaryOperatorEvaluator.Evaluate( this, node, abortTraverseToken );

    public override IAstNode Visit( AstBitwiseXorExpressionNode node, AbortTraverseToken abortTraverseToken )
        => NumericBinaryOperatorEvaluator.Evaluate( this, node, abortTraverseToken );

    #endregion ~Binary Operators (Bitwise)
}
