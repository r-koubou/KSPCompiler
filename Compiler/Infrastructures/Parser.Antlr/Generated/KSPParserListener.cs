//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.9.3
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from KSPParser.g4 by ANTLR 4.9.3

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

namespace KSPCompiler.Infrastructures.Parser.Antlr {
using Antlr4.Runtime.Misc;
using IParseTreeListener = Antlr4.Runtime.Tree.IParseTreeListener;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete listener for a parse tree produced by
/// <see cref="KSPParser"/>.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.9.3")]
[System.CLSCompliant(false)]
public interface IKSPParserListener : IParseTreeListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.compilationUnit"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterCompilationUnit([NotNull] KSPParser.CompilationUnitContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.compilationUnit"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitCompilationUnit([NotNull] KSPParser.CompilationUnitContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.callbackDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterCallbackDeclaration([NotNull] KSPParser.CallbackDeclarationContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.callbackDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitCallbackDeclaration([NotNull] KSPParser.CallbackDeclarationContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.argumentDefinitionList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterArgumentDefinitionList([NotNull] KSPParser.ArgumentDefinitionListContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.argumentDefinitionList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitArgumentDefinitionList([NotNull] KSPParser.ArgumentDefinitionListContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.userFunctionDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterUserFunctionDeclaration([NotNull] KSPParser.UserFunctionDeclarationContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.userFunctionDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitUserFunctionDeclaration([NotNull] KSPParser.UserFunctionDeclarationContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.block"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterBlock([NotNull] KSPParser.BlockContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.block"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitBlock([NotNull] KSPParser.BlockContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.variableDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterVariableDeclaration([NotNull] KSPParser.VariableDeclarationContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.variableDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitVariableDeclaration([NotNull] KSPParser.VariableDeclarationContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.variableInitializer"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterVariableInitializer([NotNull] KSPParser.VariableInitializerContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.variableInitializer"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitVariableInitializer([NotNull] KSPParser.VariableInitializerContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.primitiveInitializer"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterPrimitiveInitializer([NotNull] KSPParser.PrimitiveInitializerContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.primitiveInitializer"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitPrimitiveInitializer([NotNull] KSPParser.PrimitiveInitializerContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.arrayInitializer"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterArrayInitializer([NotNull] KSPParser.ArrayInitializerContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.arrayInitializer"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitArrayInitializer([NotNull] KSPParser.ArrayInitializerContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterStatement([NotNull] KSPParser.StatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitStatement([NotNull] KSPParser.StatementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.kspPreprocessor"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterKspPreprocessor([NotNull] KSPParser.KspPreprocessorContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.kspPreprocessor"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitKspPreprocessor([NotNull] KSPParser.KspPreprocessorContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.kspPreprocessorDefine"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterKspPreprocessorDefine([NotNull] KSPParser.KspPreprocessorDefineContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.kspPreprocessorDefine"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitKspPreprocessorDefine([NotNull] KSPParser.KspPreprocessorDefineContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.kspPreprocessorUndefine"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterKspPreprocessorUndefine([NotNull] KSPParser.KspPreprocessorUndefineContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.kspPreprocessorUndefine"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitKspPreprocessorUndefine([NotNull] KSPParser.KspPreprocessorUndefineContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.kspPreprocessorIfdefine"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterKspPreprocessorIfdefine([NotNull] KSPParser.KspPreprocessorIfdefineContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.kspPreprocessorIfdefine"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitKspPreprocessorIfdefine([NotNull] KSPParser.KspPreprocessorIfdefineContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.kspPreprocessorIfnotDefine"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterKspPreprocessorIfnotDefine([NotNull] KSPParser.KspPreprocessorIfnotDefineContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.kspPreprocessorIfnotDefine"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitKspPreprocessorIfnotDefine([NotNull] KSPParser.KspPreprocessorIfnotDefineContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.ifStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterIfStatement([NotNull] KSPParser.IfStatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.ifStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitIfStatement([NotNull] KSPParser.IfStatementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.selectStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterSelectStatement([NotNull] KSPParser.SelectStatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.selectStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitSelectStatement([NotNull] KSPParser.SelectStatementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.caseBlock"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterCaseBlock([NotNull] KSPParser.CaseBlockContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.caseBlock"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitCaseBlock([NotNull] KSPParser.CaseBlockContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.whileStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterWhileStatement([NotNull] KSPParser.WhileStatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.whileStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitWhileStatement([NotNull] KSPParser.WhileStatementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.continueStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterContinueStatement([NotNull] KSPParser.ContinueStatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.continueStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitContinueStatement([NotNull] KSPParser.ContinueStatementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.callKspUserFunction"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterCallKspUserFunction([NotNull] KSPParser.CallKspUserFunctionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.callKspUserFunction"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitCallKspUserFunction([NotNull] KSPParser.CallKspUserFunctionContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.expressionStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterExpressionStatement([NotNull] KSPParser.ExpressionStatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.expressionStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitExpressionStatement([NotNull] KSPParser.ExpressionStatementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.primaryExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterPrimaryExpression([NotNull] KSPParser.PrimaryExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.primaryExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitPrimaryExpression([NotNull] KSPParser.PrimaryExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.postfixExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterPostfixExpression([NotNull] KSPParser.PostfixExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.postfixExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitPostfixExpression([NotNull] KSPParser.PostfixExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.assignmentExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterAssignmentExpression([NotNull] KSPParser.AssignmentExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.assignmentExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitAssignmentExpression([NotNull] KSPParser.AssignmentExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.assignmentExpressionList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterAssignmentExpressionList([NotNull] KSPParser.AssignmentExpressionListContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.assignmentExpressionList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitAssignmentExpressionList([NotNull] KSPParser.AssignmentExpressionListContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.assignmentOperator"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterAssignmentOperator([NotNull] KSPParser.AssignmentOperatorContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.assignmentOperator"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitAssignmentOperator([NotNull] KSPParser.AssignmentOperatorContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterExpression([NotNull] KSPParser.ExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitExpression([NotNull] KSPParser.ExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.expressionList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterExpressionList([NotNull] KSPParser.ExpressionListContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.expressionList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitExpressionList([NotNull] KSPParser.ExpressionListContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.stringConcatenateExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterStringConcatenateExpression([NotNull] KSPParser.StringConcatenateExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.stringConcatenateExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitStringConcatenateExpression([NotNull] KSPParser.StringConcatenateExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.logicalOrExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterLogicalOrExpression([NotNull] KSPParser.LogicalOrExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.logicalOrExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitLogicalOrExpression([NotNull] KSPParser.LogicalOrExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.logicalAndExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterLogicalAndExpression([NotNull] KSPParser.LogicalAndExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.logicalAndExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitLogicalAndExpression([NotNull] KSPParser.LogicalAndExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.bitwiseOrExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterBitwiseOrExpression([NotNull] KSPParser.BitwiseOrExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.bitwiseOrExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitBitwiseOrExpression([NotNull] KSPParser.BitwiseOrExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.bitwiseAndExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterBitwiseAndExpression([NotNull] KSPParser.BitwiseAndExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.bitwiseAndExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitBitwiseAndExpression([NotNull] KSPParser.BitwiseAndExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.equalityExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterEqualityExpression([NotNull] KSPParser.EqualityExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.equalityExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitEqualityExpression([NotNull] KSPParser.EqualityExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.relationalExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterRelationalExpression([NotNull] KSPParser.RelationalExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.relationalExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitRelationalExpression([NotNull] KSPParser.RelationalExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.additiveExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterAdditiveExpression([NotNull] KSPParser.AdditiveExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.additiveExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitAdditiveExpression([NotNull] KSPParser.AdditiveExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.multiplicativeExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMultiplicativeExpression([NotNull] KSPParser.MultiplicativeExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.multiplicativeExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMultiplicativeExpression([NotNull] KSPParser.MultiplicativeExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.unaryExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterUnaryExpression([NotNull] KSPParser.UnaryExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.unaryExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitUnaryExpression([NotNull] KSPParser.UnaryExpressionContext context);
}
} // namespace KSPCompiler.Infrastructures.Parser.Antlr
