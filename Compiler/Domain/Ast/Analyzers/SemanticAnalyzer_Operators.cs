using System;

using KSPCompiler.Domain.Ast.Node;
using KSPCompiler.Domain.Ast.Node.Expressions;

namespace KSPCompiler.Domain.Ast.Analyzers;

public partial class SemanticAnalyzer : DefaultAstVisitor, ISemanticAnalyzer
{
    #region Binary Operators (Mathematical)

    public override IAstNode Visit( AstAdditionExpression node )
        => throw new NotImplementedException("operator +");

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
}
