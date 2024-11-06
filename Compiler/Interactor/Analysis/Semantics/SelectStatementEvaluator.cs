using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Extensions;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Interactor.Analysis.Commons.Evaluations;
using KSPCompiler.Interactor.Analysis.Commons.Extensions;
using KSPCompiler.Resources;
using KSPCompiler.UseCases.Analysis.Evaluations.Conditionals;

namespace KSPCompiler.Interactor.Analysis.Semantics;

public class SelectStatementEvaluator : ISelectStatementEvaluator
{
    private ICompilerMessageManger CompilerMessageManger { get; }

    public SelectStatementEvaluator( ICompilerMessageManger compilerMessageManger )
    {
        CompilerMessageManger = compilerMessageManger;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstSelectStatementNode statement )
    {
        if( statement.Condition.Accept( visitor ) is not AstExpressionNode evaluatedCondition )
        {
            throw new AstAnalyzeException( statement, "Failed to evaluate condition" );
        }

        // selectの評価対象が変数ではない場合
        if( evaluatedCondition is not AstSymbolExpressionNode )
        {
            CompilerMessageManger.Error(
                statement,
                CompilerMessageResources.semantic_error_select_condition_notvariable
            );

            return statement.Clone<AstSelectStatementNode>();
        }

        // selectの評価対象が整数ではない場合
        if( evaluatedCondition.TypeFlag != DataTypeFlag.TypeInt )
        {
            CompilerMessageManger.Error(
                statement,
                CompilerMessageResources.semantic_error_select_condition_incompatible
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
            CompilerMessageManger.Error(
                statement,
                CompilerMessageResources.semantic_error_select_case_incompatible
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
                CompilerMessageManger.Error(
                    statement,
                    CompilerMessageResources.semantic_error_select_case_incompatible
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
            CompilerMessageManger.Warning(
                caseFrom,
                CompilerMessageResources.semantic_warning_select_case_from_to_noeffect,
                caseFromLiteral.Value
            );

            return;
        }

        // case <from> が case <to> より大きい場合
        if( caseFromLiteral.Value > caseToLiteral.Value )
        {
            CompilerMessageManger.Error(
                caseFrom,
                CompilerMessageResources.semantic_error_select_case_from_grater,
                caseFromLiteral.Value
            );
        }
    }
}
