using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Statements;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Commons.Evaluations;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Extensions;
using KSPCompiler.Resources;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Statements;
using KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData.Extensions;
using KSPCompiler.Shared.EventEmitting;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Semantics;

public class IfStatementEvaluator : IIfStatementEvaluator
{
    private IEventEmitter EventEmitter { get; }

    public IfStatementEvaluator( IEventEmitter eventEmitter )
    {
        EventEmitter = eventEmitter;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstIfStatementNode statement )
    {
        // 条件式の評価
        if( statement.Condition.Accept( visitor ) is not AstExpressionNode evaluatedCondition )
        {
            throw new AstAnalyzeException( statement, "Failed to evaluate condition" );
        }

        // if条件式が条件式以外の場合
        if( !evaluatedCondition.TypeFlag.IsBoolean() )
        {
            EventEmitter.Emit(
                statement.AsErrorEvent(
                    CompilerMessageResources.semantic_error_if_condition_incompatible
                )
            );

            return statement.Clone<AstIfStatementNode>();
        }

        // 真の場合のコードブロックの評価
        statement.CodeBlock.AcceptChildren( visitor );

        // else節がある場合のコードブロックの評価
        statement.ElseBlock.AcceptChildren( visitor );

        // Memo
        // 畳み込みで条件式をリテラルでノードを置き換え可能だが
        // 文法上は条件式リテラルが許されていないため、ここでは畳み込みは行わない
        // 最適化やオブファスケーターの団で畳み込みを行う

        return statement.Clone<AstIfStatementNode>();
    }
}
