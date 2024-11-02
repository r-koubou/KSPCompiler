using KSPCompiler.Domain.Ast.Extensions;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;
using KSPCompiler.Resources;

namespace KSPCompiler.Domain.Ast.Analyzers.Evaluators.Conditionals;

public class WhileStatementEvaluator : IWhileStatementEvaluator
{
    private ICompilerMessageManger CompilerMessageManger { get; }

    public WhileStatementEvaluator( ICompilerMessageManger compilerMessageManger )
    {
        CompilerMessageManger = compilerMessageManger;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstWhileStatementNode statement )
    {
        // 条件式の評価
        if( statement.Condition.Accept( visitor ) is not AstExpressionNode evaluatedCondition )
        {
            throw new AstAnalyzeException( statement, "Failed to evaluate condition" );
        }

        // if条件式が条件式以外の場合
        if( !evaluatedCondition.TypeFlag.IsBoolean() )
        {
            CompilerMessageManger.Error(
                statement,
                CompilerMessageResources.semantic_error_if_condition_incompatible
            );

            return statement.Clone<AstWhileStatementNode>();
        }

        // 真の場合のコードブロックの評価
        statement.CodeBlock.AcceptChildren( visitor );

        // Memo
        // 畳み込みで条件式をリテラルでノードを置き換え可能だが
        // 文法上は条件式リテラルが許されていないため、ここでは畳み込みは行わない
        // 最適化やオブファスケーターの団で畳み込みを行う

        return statement.Clone<AstWhileStatementNode>();
    }
}
