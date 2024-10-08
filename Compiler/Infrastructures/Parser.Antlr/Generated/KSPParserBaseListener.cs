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
using IErrorNode = Antlr4.Runtime.Tree.IErrorNode;
using ITerminalNode = Antlr4.Runtime.Tree.ITerminalNode;
using IToken = Antlr4.Runtime.IToken;
using ParserRuleContext = Antlr4.Runtime.ParserRuleContext;

/// <summary>
/// This class provides an empty implementation of <see cref="IKSPParserListener"/>,
/// which can be extended to create a listener which only needs to handle a subset
/// of the available methods.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.9.3")]
[System.Diagnostics.DebuggerNonUserCode]
[System.CLSCompliant(false)]
public partial class KSPParserBaseListener : IKSPParserListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.compilationUnit"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterCompilationUnit([NotNull] KSPParser.CompilationUnitContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.compilationUnit"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitCompilationUnit([NotNull] KSPParser.CompilationUnitContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.callbackDeclaration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterCallbackDeclaration([NotNull] KSPParser.CallbackDeclarationContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.callbackDeclaration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitCallbackDeclaration([NotNull] KSPParser.CallbackDeclarationContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.argumentDefinitionList"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterArgumentDefinitionList([NotNull] KSPParser.ArgumentDefinitionListContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.argumentDefinitionList"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitArgumentDefinitionList([NotNull] KSPParser.ArgumentDefinitionListContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.userFunctionDeclaration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterUserFunctionDeclaration([NotNull] KSPParser.UserFunctionDeclarationContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.userFunctionDeclaration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitUserFunctionDeclaration([NotNull] KSPParser.UserFunctionDeclarationContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.block"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterBlock([NotNull] KSPParser.BlockContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.block"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitBlock([NotNull] KSPParser.BlockContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.variableDeclaration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterVariableDeclaration([NotNull] KSPParser.VariableDeclarationContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.variableDeclaration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitVariableDeclaration([NotNull] KSPParser.VariableDeclarationContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.variableInitializer"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterVariableInitializer([NotNull] KSPParser.VariableInitializerContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.variableInitializer"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitVariableInitializer([NotNull] KSPParser.VariableInitializerContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.primitiveInitializer"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterPrimitiveInitializer([NotNull] KSPParser.PrimitiveInitializerContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.primitiveInitializer"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitPrimitiveInitializer([NotNull] KSPParser.PrimitiveInitializerContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.arrayInitializer"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterArrayInitializer([NotNull] KSPParser.ArrayInitializerContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.arrayInitializer"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitArrayInitializer([NotNull] KSPParser.ArrayInitializerContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.statement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterStatement([NotNull] KSPParser.StatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.statement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitStatement([NotNull] KSPParser.StatementContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.kspPreprocessor"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterKspPreprocessor([NotNull] KSPParser.KspPreprocessorContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.kspPreprocessor"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitKspPreprocessor([NotNull] KSPParser.KspPreprocessorContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.kspPreprocessorDefine"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterKspPreprocessorDefine([NotNull] KSPParser.KspPreprocessorDefineContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.kspPreprocessorDefine"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitKspPreprocessorDefine([NotNull] KSPParser.KspPreprocessorDefineContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.kspPreprocessorUndefine"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterKspPreprocessorUndefine([NotNull] KSPParser.KspPreprocessorUndefineContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.kspPreprocessorUndefine"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitKspPreprocessorUndefine([NotNull] KSPParser.KspPreprocessorUndefineContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.kspPreprocessorIfdefine"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterKspPreprocessorIfdefine([NotNull] KSPParser.KspPreprocessorIfdefineContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.kspPreprocessorIfdefine"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitKspPreprocessorIfdefine([NotNull] KSPParser.KspPreprocessorIfdefineContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.kspPreprocessorIfnotDefine"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterKspPreprocessorIfnotDefine([NotNull] KSPParser.KspPreprocessorIfnotDefineContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.kspPreprocessorIfnotDefine"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitKspPreprocessorIfnotDefine([NotNull] KSPParser.KspPreprocessorIfnotDefineContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.ifStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterIfStatement([NotNull] KSPParser.IfStatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.ifStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitIfStatement([NotNull] KSPParser.IfStatementContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.selectStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterSelectStatement([NotNull] KSPParser.SelectStatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.selectStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitSelectStatement([NotNull] KSPParser.SelectStatementContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.caseBlock"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterCaseBlock([NotNull] KSPParser.CaseBlockContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.caseBlock"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitCaseBlock([NotNull] KSPParser.CaseBlockContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.whileStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterWhileStatement([NotNull] KSPParser.WhileStatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.whileStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitWhileStatement([NotNull] KSPParser.WhileStatementContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.continueStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterContinueStatement([NotNull] KSPParser.ContinueStatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.continueStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitContinueStatement([NotNull] KSPParser.ContinueStatementContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.callKspUserFunction"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterCallKspUserFunction([NotNull] KSPParser.CallKspUserFunctionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.callKspUserFunction"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitCallKspUserFunction([NotNull] KSPParser.CallKspUserFunctionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.expressionStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterExpressionStatement([NotNull] KSPParser.ExpressionStatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.expressionStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitExpressionStatement([NotNull] KSPParser.ExpressionStatementContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.primaryExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterPrimaryExpression([NotNull] KSPParser.PrimaryExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.primaryExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitPrimaryExpression([NotNull] KSPParser.PrimaryExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.postfixExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterPostfixExpression([NotNull] KSPParser.PostfixExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.postfixExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitPostfixExpression([NotNull] KSPParser.PostfixExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.assignmentExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterAssignmentExpression([NotNull] KSPParser.AssignmentExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.assignmentExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitAssignmentExpression([NotNull] KSPParser.AssignmentExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.assignmentExpressionList"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterAssignmentExpressionList([NotNull] KSPParser.AssignmentExpressionListContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.assignmentExpressionList"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitAssignmentExpressionList([NotNull] KSPParser.AssignmentExpressionListContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.assignmentOperator"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterAssignmentOperator([NotNull] KSPParser.AssignmentOperatorContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.assignmentOperator"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitAssignmentOperator([NotNull] KSPParser.AssignmentOperatorContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterExpression([NotNull] KSPParser.ExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitExpression([NotNull] KSPParser.ExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.expressionList"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterExpressionList([NotNull] KSPParser.ExpressionListContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.expressionList"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitExpressionList([NotNull] KSPParser.ExpressionListContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.stringConcatenateExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterStringConcatenateExpression([NotNull] KSPParser.StringConcatenateExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.stringConcatenateExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitStringConcatenateExpression([NotNull] KSPParser.StringConcatenateExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.logicalOrExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterLogicalOrExpression([NotNull] KSPParser.LogicalOrExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.logicalOrExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitLogicalOrExpression([NotNull] KSPParser.LogicalOrExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.logicalAndExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterLogicalAndExpression([NotNull] KSPParser.LogicalAndExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.logicalAndExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitLogicalAndExpression([NotNull] KSPParser.LogicalAndExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.logicalXorExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterLogicalXorExpression([NotNull] KSPParser.LogicalXorExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.logicalXorExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitLogicalXorExpression([NotNull] KSPParser.LogicalXorExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.bitwiseOrExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterBitwiseOrExpression([NotNull] KSPParser.BitwiseOrExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.bitwiseOrExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitBitwiseOrExpression([NotNull] KSPParser.BitwiseOrExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.bitwiseAndExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterBitwiseAndExpression([NotNull] KSPParser.BitwiseAndExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.bitwiseAndExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitBitwiseAndExpression([NotNull] KSPParser.BitwiseAndExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.bitwiseXorExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterBitwiseXorExpression([NotNull] KSPParser.BitwiseXorExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.bitwiseXorExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitBitwiseXorExpression([NotNull] KSPParser.BitwiseXorExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.equalityExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterEqualityExpression([NotNull] KSPParser.EqualityExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.equalityExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitEqualityExpression([NotNull] KSPParser.EqualityExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.relationalExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterRelationalExpression([NotNull] KSPParser.RelationalExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.relationalExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitRelationalExpression([NotNull] KSPParser.RelationalExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.additiveExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterAdditiveExpression([NotNull] KSPParser.AdditiveExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.additiveExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitAdditiveExpression([NotNull] KSPParser.AdditiveExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.multiplicativeExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterMultiplicativeExpression([NotNull] KSPParser.MultiplicativeExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.multiplicativeExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitMultiplicativeExpression([NotNull] KSPParser.MultiplicativeExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="KSPParser.unaryExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterUnaryExpression([NotNull] KSPParser.UnaryExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="KSPParser.unaryExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitUnaryExpression([NotNull] KSPParser.UnaryExpressionContext context) { }

	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void EnterEveryRule([NotNull] ParserRuleContext context) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void ExitEveryRule([NotNull] ParserRuleContext context) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void VisitTerminal([NotNull] ITerminalNode node) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void VisitErrorNode([NotNull] IErrorNode node) { }
}
} // namespace KSPCompiler.Infrastructures.Parser.Antlr
