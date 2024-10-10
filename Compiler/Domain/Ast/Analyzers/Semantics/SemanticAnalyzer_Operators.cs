using System;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Domain.Ast.Analyzers.Semantics;

public partial class SemanticAnalyzer
{
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

    public override IAstNode Visit( AstStringConcatenateExpressionNode node, AbortTraverseToken abortTraverseToken )
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

    #region Unary Operators

    public override IAstNode Visit( AstUnaryNotExpressionNode node, AbortTraverseToken abortTraverseToken )
        // => throw new NotImplementedException();
        => NumericUnaryOperatorEvaluator.Evaluate( this, node, abortTraverseToken );

    public override IAstNode Visit( AstUnaryMinusExpressionNode node, AbortTraverseToken abortTraverseToken )
        //=> throw new NotImplementedException();
        => NumericUnaryOperatorEvaluator.Evaluate( this, node, abortTraverseToken );

    #endregion ~Unary Operators


    public override IAstNode Visit( AstAssignmentExpressionNode node, AbortTraverseToken abortTraverseToken )
    {
        var resultL = node.Left.Accept( this, abortTraverseToken );
        var resultR = node.Right.Accept( this, abortTraverseToken );

        return node;
    }
}
