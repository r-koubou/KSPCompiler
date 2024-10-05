using System;

using KSPCompiler.Domain.Ast.Extensions;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;
using KSPCompiler.Resources;

namespace KSPCompiler.Domain.Ast.Analyzers;

public partial class SemanticAnalyzer
{
    #region Binary Operators (Mathematical)

    public override IAstNode Visit( AstAdditionExpression node, AbortTraverseToken abortTraverseToken )
    {
        if( node.Left.Accept( this, abortTraverseToken ) is not AstExpressionSyntaxNode left )
        {
            throw new AstAnalyzeException( this,  node, $"Left side of expression is invalid" );
        }

        if( node.Right.Accept( this, abortTraverseToken ) is not AstExpressionSyntaxNode right )
        {
            throw new AstAnalyzeException( this,  node, $"Right side of expression is invalid" );
        }

        if( abortTraverseToken.Aborted )
        {
            return NullAstNode.Instance;
        }

        var leftType = left.TypeFlag;
        var rightType = right.TypeFlag;

        if( leftType.IsNumerical() && rightType.IsNumerical() && leftType == rightType )
        {
            return node;
        }

        CompilerMessageManger.Error(
            node,
            CompilerMessageResources.semantic_error_binaryoprator_compatible,
            leftType.ToMessageString(),
            rightType.ToMessageString()
        );

        abortTraverseToken.Abort();
        return NullAstNode.Instance;
    }

    public override IAstNode Visit( AstSubtractionExpression node, AbortTraverseToken abortTraverseToken )
        => throw new NotImplementedException( "operator -" );

    public override IAstNode Visit( AstMultiplyingExpression node, AbortTraverseToken abortTraverseToken )
        => throw new NotImplementedException( "operator *" );

    public override IAstNode Visit( AstDivisionExpression node, AbortTraverseToken abortTraverseToken )
        => throw new NotImplementedException( "operator /" );

    public override IAstNode Visit( AstModuloExpression node, AbortTraverseToken abortTraverseToken )
        => throw new NotImplementedException( "operator mod" );

    public override IAstNode Visit( AstStringConcatenateExpression node, AbortTraverseToken abortTraverseToken )
        => throw new NotImplementedException( "operator &" );
    #endregion ~Binary Operators

    #region Binary Operators (Bitwise)
    public override IAstNode Visit( AstBitwiseOrExpression node, AbortTraverseToken abortTraverseToken )
        => throw new NotImplementedException( "operator bitwise or (.or.)" );

    public override IAstNode Visit( AstBitwiseAndExpression node, AbortTraverseToken abortTraverseToken )
        => throw new NotImplementedException( "operator bitwise and (.and.)" );

    public override IAstNode Visit( AstUnaryNotExpression node, AbortTraverseToken abortTraverseToken )
        => throw new NotImplementedException( "operator bitwise not (.not.)" );
    #endregion

    #region Unary Operators
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
