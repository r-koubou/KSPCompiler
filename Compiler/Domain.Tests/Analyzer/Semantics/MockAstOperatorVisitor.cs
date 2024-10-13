using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Operators;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

public class MockAstOperatorVisitor : DefaultAstVisitor
{
    private IBinaryOperatorEvaluator NumericBinaryOperatorEvaluator { get; }
    private IUnaryOperatorEvaluator NumericUnaryOperatorEvaluator { get; }
    private IStringConcatenateOperatorEvaluator StringConcatenateOperatorEvaluator { get; }

    public MockAstOperatorVisitor(
        IBinaryOperatorEvaluator numericBinaryOperatorEvaluator,
        IUnaryOperatorEvaluator numericUnaryOperatorEvaluator,
        IStringConcatenateOperatorEvaluator stringConcatenateOperatorEvaluator )
    {
        NumericBinaryOperatorEvaluator     = numericBinaryOperatorEvaluator;
        NumericUnaryOperatorEvaluator      = numericUnaryOperatorEvaluator;
        StringConcatenateOperatorEvaluator = stringConcatenateOperatorEvaluator;
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

    #region String concatenation operator

    public override IAstNode Visit( AstStringConcatenateExpressionNode node, AbortTraverseToken abortTraverseToken )
        => StringConcatenateOperatorEvaluator.Evaluate( this, node, abortTraverseToken );

    #endregion

    #region Unary Operators

    public override IAstNode Visit( AstUnaryNotExpressionNode node, AbortTraverseToken abortTraverseToken )
        => NumericUnaryOperatorEvaluator.Evaluate( this, node, abortTraverseToken );

    public override IAstNode Visit( AstUnaryMinusExpressionNode node, AbortTraverseToken abortTraverseToken )
        => NumericUnaryOperatorEvaluator.Evaluate( this, node, abortTraverseToken );

    #endregion ~Unary Operators

}
