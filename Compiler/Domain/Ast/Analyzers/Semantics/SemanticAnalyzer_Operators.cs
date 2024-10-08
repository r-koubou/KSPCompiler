using System;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Domain.Ast.Analyzers.Semantics;

public partial class SemanticAnalyzer
{
    #region Binary Operators (Mathematical)

    public override IAstNode Visit( AstAdditionExpression node, AbortTraverseToken abortTraverseToken )
        => NumericBinaryOperatorEvaluator.Evaluate( this, node, abortTraverseToken );

    public override IAstNode Visit( AstSubtractionExpression node, AbortTraverseToken abortTraverseToken )
        => NumericBinaryOperatorEvaluator.Evaluate( this, node, abortTraverseToken );

    public override IAstNode Visit( AstMultiplyingExpression node, AbortTraverseToken abortTraverseToken )
        => NumericBinaryOperatorEvaluator.Evaluate( this, node, abortTraverseToken );

    public override IAstNode Visit( AstDivisionExpression node, AbortTraverseToken abortTraverseToken )
        => NumericBinaryOperatorEvaluator.Evaluate( this, node, abortTraverseToken );

    public override IAstNode Visit( AstModuloExpression node, AbortTraverseToken abortTraverseToken )
        => NumericBinaryOperatorEvaluator.Evaluate( this, node, abortTraverseToken );

    public override IAstNode Visit( AstStringConcatenateExpression node, AbortTraverseToken abortTraverseToken )
        => NumericBinaryOperatorEvaluator.Evaluate( this, node, abortTraverseToken );

    #endregion ~Binary Operators

    #region Binary Operators (Bitwise)
    public override IAstNode Visit( AstBitwiseOrExpression node, AbortTraverseToken abortTraverseToken )
        => NumericBinaryOperatorEvaluator.Evaluate( this, node, abortTraverseToken );

    public override IAstNode Visit( AstBitwiseAndExpression node, AbortTraverseToken abortTraverseToken )
        => NumericBinaryOperatorEvaluator.Evaluate( this, node, abortTraverseToken );

    public override IAstNode Visit( AstBitwiseXorExpression node, AbortTraverseToken abortTraverseToken )
        => NumericBinaryOperatorEvaluator.Evaluate( this, node, abortTraverseToken );


    #endregion ~Binary Operators

    #region Unary Operators

    public override IAstNode Visit( AstUnaryNotExpression node, AbortTraverseToken abortTraverseToken )
        => throw new NotImplementedException( "operator bitwise not (.not.)" );

    public override IAstNode Visit( AstUnaryMinusExpression node, AbortTraverseToken abortTraverseToken )
        => throw new NotImplementedException( "operator unary -" );

    #endregion ~Unary Operators


    public override IAstNode Visit( AstAssignmentExpression node, AbortTraverseToken abortTraverseToken )
    {
        var resultL = node.Left.Accept( this, abortTraverseToken );
        var resultR = node.Right.Accept( this, abortTraverseToken );

        return node;
    }
}
