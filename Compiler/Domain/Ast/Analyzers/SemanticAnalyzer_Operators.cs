using System;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;

namespace KSPCompiler.Domain.Ast.Analyzers;

public partial class SemanticAnalyzer
{
    #region Binary Operators (Mathematical)

    public override IAstNode Visit( AstAdditionExpression node )
    {
        var left = (AstExpressionSyntaxNode)node.Left.Accept( this );
        var right = (AstExpressionSyntaxNode)node.Right.Accept( this );
        var leftType = left.TypeFlag;
        var rightType = right.TypeFlag;

        if( !leftType.IsNumerical() || !rightType.IsNumerical() )
        {
            return new AstIntLiteral();
        }

        throw new NotImplementedException( "operator +" );
    }

    public override IAstNode Visit( AstSubtractionExpression node )
        => throw new NotImplementedException("operator -");

    public override IAstNode Visit( AstMultiplyingExpression node )
        => throw new NotImplementedException("operator *");

    public override IAstNode Visit( AstDivisionExpression node )
        => throw new NotImplementedException("operator /");

    public override IAstNode Visit( AstModuloExpression node )
        => throw new NotImplementedException("operator mod");

    public override IAstNode Visit( AstStringConcatenateExpression node )
        => throw new NotImplementedException("operator &");

    #endregion ~Binary Operators

    #region Binary Operators (Bitwise)

    public override IAstNode Visit( AstBitwiseOrExpression node )
        => throw new NotImplementedException("operator bitwise or (.or.)");

    public override IAstNode Visit( AstBitwiseAndExpression node )
        => throw new NotImplementedException("operator bitwise and (.and.)");

    public override IAstNode Visit( AstUnaryNotExpression node )
        => throw new NotImplementedException("operator bitwise not (.not.)");

    #endregion

    #region Unary Operators

    public override IAstNode Visit( AstUnaryMinusExpression node )
        => throw new NotImplementedException("operator unary -");

    #endregion ~Unary Operators

    public override IAstNode Visit( AstAssignmentExpression node )
    {
        var resultL = node.Left.Accept( this );
        var resultR = node.Right.Accept( this );
        return node;
    }
}
