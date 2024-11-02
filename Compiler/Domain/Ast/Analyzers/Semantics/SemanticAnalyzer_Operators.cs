using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Domain.Ast.Analyzers.Semantics;

public partial class SemanticAnalyzer
{
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

    #region String concatenation operator

    public override IAstNode Visit( AstStringConcatenateExpressionNode node )
        => StringConcatenateOperatorEvaluator.Evaluate( this, node );

    #endregion

    #region Unary Operators

    public override IAstNode Visit( AstUnaryNotExpressionNode node )
        // => throw new NotImplementedException();
        => NumericUnaryOperatorEvaluator.Evaluate( this, node );

    public override IAstNode Visit( AstUnaryMinusExpressionNode node )
        //=> throw new NotImplementedException();
        => NumericUnaryOperatorEvaluator.Evaluate( this, node );

    #endregion ~Unary Operators


    public override IAstNode Visit( AstAssignStatementNode node )
        => AssignStatementEvaluator.Evaluate( this, node );
}
