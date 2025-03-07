using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Statements;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Commons.Evaluations;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Extensions;
using KSPCompiler.Resources;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Blocks;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Extensions;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Statements;
using KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData;
using KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData.Extensions;
using KSPCompiler.Shared.EventEmitting;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Semantics;

public class SelectStatementEvaluator : ISelectStatementEvaluator
{
    private IEventEmitter EventEmitter { get; }

    public SelectStatementEvaluator( IEventEmitter eventEmitter )
    {
        EventEmitter = eventEmitter;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstSelectStatementNode statement )
    {
        if( statement.Condition.Accept( visitor ) is not AstExpressionNode evaluatedCondition )
        {
            throw new AstAnalyzeException( statement, "Failed to evaluate condition" );
        }

        // selectの評価対象が整数ではない場合
        if( !evaluatedCondition.TypeFlag.IsInt() )
        {
            EventEmitter.Emit(
                statement.AsErrorEvent(
                    CompilerMessageResources.semantic_error_select_condition_incompatible
                )
            );

            return statement.Clone<AstSelectStatementNode>();
        }

        foreach( var caseBlock in statement.CaseBlocks )
        {
            EvaluateCaseBlock( visitor, statement, caseBlock );
        }

        return statement.Clone<AstSelectStatementNode>();
    }

    private void EvaluateCaseBlock( IAstVisitor visitor, AstSelectStatementNode statement, AstCaseBlock caseBlock )
    {
        if( caseBlock.ConditionFrom.Accept( visitor ) is not AstExpressionNode evaluatedCaseFrom )
        {
            throw new AstAnalyzeException( statement, "Failed to evaluate case condition" );
        }

        // case 条件が整数かつ定数ではない場合
        if( evaluatedCaseFrom.TypeFlag != DataTypeFlag.TypeInt || !evaluatedCaseFrom.Constant )
        {
            EventEmitter.Emit(
                statement.AsErrorEvent(
                    CompilerMessageResources.semantic_error_select_case_incompatible
                )
            );
        }

        // from to の場合
        if( caseBlock.ConditionTo.IsNotNull() )
        {
            if( caseBlock.ConditionTo.Accept( visitor ) is not AstExpressionNode evaluatedCaseTo )
            {
                throw new AstAnalyzeException( statement, "Failed to evaluate case condition" );
            }

            // case 条件が整数かつ定数ではない場合
            if( evaluatedCaseTo.TypeFlag != DataTypeFlag.TypeInt || !evaluatedCaseTo.Constant )
            {
                EventEmitter.Emit(
                    statement.AsErrorEvent(
                        CompilerMessageResources.semantic_error_select_case_incompatible
                    )
                );

                return;
            }

            EvaluateCaseRange( evaluatedCaseFrom, evaluatedCaseTo );
        }

        // case 内のコードブロックの評価
        caseBlock.CodeBlock.AcceptChildren( visitor );
    }

    // from to の値が確定している場合の範囲が正しいかチェック
    private void EvaluateCaseRange( AstExpressionNode caseFrom, AstExpressionNode caseTo )
    {
        if( caseFrom is not AstIntLiteralNode caseFromLiteral )
        {
            return;
        }

        if( caseTo is not AstIntLiteralNode caseToLiteral )
        {
            return;
        }

        // case <from> と case <to> が同じ値の場合
        if( caseFromLiteral.Value == caseToLiteral.Value )
        {
            EventEmitter.Emit(
                caseFrom.AsWarningEvent(
                    CompilerMessageResources.semantic_warning_select_case_from_to_noeffect,
                    caseFromLiteral.Value
                )
            );

            return;
        }

        // case <from> が case <to> より大きい場合
        if( caseFromLiteral.Value > caseToLiteral.Value )
        {
            EventEmitter.Emit(
                caseFrom.AsErrorEvent(
                    CompilerMessageResources.semantic_error_select_case_from_grater,
                    caseFromLiteral.Value
                )
            );
        }
    }
}
