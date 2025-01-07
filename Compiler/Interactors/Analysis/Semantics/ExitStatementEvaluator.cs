using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Domain.Events;
using KSPCompiler.UseCases.Analysis.Evaluations.Statements;

namespace KSPCompiler.Interactors.Analysis.Semantics;

public class ExitStatementEvaluator : IExitStatementEvaluator
{
    private IEventEmitter EventEmitter { get; }

    public ExitStatementEvaluator( IEventEmitter eventEmitter )
    {
        EventEmitter = eventEmitter;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstExitStatementNode statement )
    {
        // 評価項目なしのため、そのままコピーを返す
        return statement.Clone<AstExitStatementNode>();
    }
}
