using KSPCompiler.Domain.Ast.Analyzers.Context;
using KSPCompiler.Domain.Ast.Analyzers.Semantics.Extensions;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Statements;

namespace KSPCompiler.Domain.Ast.Analyzers.Semantics;

public class SemanticAnalyzer : DefaultAstVisitor, IAstTraversal
{
    protected IAnalyzerContext Context { get; }

    public SemanticAnalyzer( IAnalyzerContext context )
    {
        Context = context;
    }

    public void Traverse( AstCompilationUnitNode node )
    {
        node.AcceptChildrenPreferInitCallback( this );
    }

    #region Declarations

    public override IAstNode Visit( AstCallbackDeclarationNode node )
        => Context.DeclarationContext.Callback.Evaluate( this, node );

    public override IAstNode Visit( AstUserFunctionDeclarationNode node )
        => Context.DeclarationContext.UserFunction.Evaluate( this, node );

    public override IAstNode Visit( AstVariableDeclarationNode node )
        => Context.DeclarationContext.Variable.Evaluate( this, node );

    #endregion ~Declarations

    #region Expressions

    #region Assignments

    public override IAstNode Visit( AstAssignmentExpressionNode node )
        => Context.ExpressionContext.AssignOperator.Evaluate( this, node );

    #endregion ~Assignments

    #region Operators

    #region Binary Operators

    public override IAstNode Visit( AstAdditionExpressionNode node )
        => Context.ExpressionContext.NumericBinaryOperator.Evaluate( this, node );

    public override IAstNode Visit( AstSubtractionExpressionNode node )
        => Context.ExpressionContext.NumericBinaryOperator.Evaluate( this, node );

    public override IAstNode Visit( AstMultiplyingExpressionNode node )
        => Context.ExpressionContext.NumericBinaryOperator.Evaluate( this, node );

    public override IAstNode Visit( AstDivisionExpressionNode node )
        => Context.ExpressionContext.NumericBinaryOperator.Evaluate( this, node );

    public override IAstNode Visit( AstModuloExpressionNode node )
        => Context.ExpressionContext.NumericBinaryOperator.Evaluate( this, node );

    public override IAstNode Visit( AstBitwiseOrExpressionNode node )
        => Context.ExpressionContext.NumericBinaryOperator.Evaluate( this, node );

    public override IAstNode Visit( AstBitwiseAndExpressionNode node )
        => Context.ExpressionContext.NumericBinaryOperator.Evaluate( this, node );

    public override IAstNode Visit( AstBitwiseXorExpressionNode node )
        => Context.ExpressionContext.NumericBinaryOperator.Evaluate( this, node );

    #endregion ~Binary Operators

    #region String concatenation operator

    public override IAstNode Visit( AstStringConcatenateExpressionNode node )
        => Context.ExpressionContext.StringConcatenateOperator.Evaluate( this, node );

    #region Unary Operators

    public override IAstNode Visit( AstUnaryNotExpressionNode node )
        => Context.ExpressionContext.NumericUnaryOperator.Evaluate( this, node );

    public override IAstNode Visit( AstUnaryMinusExpressionNode node )
        => Context.ExpressionContext.NumericUnaryOperator.Evaluate( this, node );

    #endregion ~Unary Operators

    #endregion

    #region Conditional Binary Operators
    public override IAstNode Visit( AstEqualExpressionNode node )
        => Context.ExpressionContext.ConditionalBinaryOperator.Evaluate( this, node );

    public override IAstNode Visit( AstNotEqualExpressionNode node )
        => Context.ExpressionContext.ConditionalBinaryOperator.Evaluate( this, node );

    public override IAstNode Visit( AstLessThanExpressionNode node )
        => Context.ExpressionContext.ConditionalBinaryOperator.Evaluate( this, node );

    public override IAstNode Visit( AstGreaterThanExpressionNode node )
        => Context.ExpressionContext.ConditionalBinaryOperator.Evaluate( this, node );

    public override IAstNode Visit( AstLessEqualExpressionNode node )
        => Context.ExpressionContext.ConditionalBinaryOperator.Evaluate( this, node );

    public override IAstNode Visit( AstGreaterEqualExpressionNode node )
        => Context.ExpressionContext.ConditionalBinaryOperator.Evaluate( this, node );

    #endregion ~Conditional Binary Operators

    #region Conditional Logical Operators

    public override IAstNode Visit( AstLogicalOrExpressionNode node )
        => Context.ExpressionContext.ConditionalLogicalOperator.Evaluate( this, node );

    public override IAstNode Visit( AstLogicalAndExpressionNode node )
        => Context.ExpressionContext.ConditionalLogicalOperator.Evaluate( this, node );

    public override IAstNode Visit( AstLogicalXorExpressionNode node )
        => Context.ExpressionContext.ConditionalLogicalOperator.Evaluate( this, node );

    public override IAstNode Visit( AstUnaryLogicalNotExpressionNode node )
        => Context.ExpressionContext.ConditionalUnaryOperator.Evaluate( this, node );

    #endregion ~Conditional Logical Operators

    #region Symbols

    public override IAstNode Visit( AstSymbolExpressionNode node )
        => Context.ExpressionContext.Symbol.Evaluate( this, node );

    public override IAstNode Visit( AstArrayElementExpressionNode node )
        => Context.ExpressionContext.ArrayElement.Evaluate( this, node );

    public override IAstNode Visit( AstCallCommandExpressionNode node )
        => Context.ExpressionContext.CallCommand.Evaluate( this, node );

    #endregion

    #endregion ~Operators

    #endregion ~Expressions

    #region Statements

    #region Preprocessor Symbol Statements

    public override IAstNode Visit( AstPreprocessorIfdefineNode node )
        => Context.StatementContext.Preprocess.Evaluate( this, node );

    public override IAstNode Visit( AstPreprocessorIfnotDefineNode node )
        => Context.StatementContext.Preprocess.Evaluate( this, node );

    #endregion ~Preprocessor Symbol Statements

    #region Call User Function Statements

    public override IAstNode Visit( AstCallUserFunctionStatementNode node )
        => Context.StatementContext.CallUserFunction.Evaluate( this, node );

    #endregion ~Call User Function Statements

    #region Control Statements

    public override IAstNode Visit( AstIfStatementNode node )
        => Context.StatementContext.If.Evaluate( this, node );

    public override IAstNode Visit( AstWhileStatementNode node )
        => Context.StatementContext.While.Evaluate( this, node );

    public override IAstNode Visit( AstSelectStatementNode node )
        => Context.StatementContext.Select.Evaluate( this, node );

    public override IAstNode Visit( AstContinueStatementNode node )
        => Context.StatementContext.Continue.Evaluate( this, node );

    #endregion ~Control Statements

    #endregion ~Statements

}
