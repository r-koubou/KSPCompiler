using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Gateways.EventEmitting;
using KSPCompiler.Interactors.Analysis.Extensions;
using KSPCompiler.Resources;
using KSPCompiler.UseCases.Analysis.Evaluations.Statements;

namespace KSPCompiler.Interactors.Analysis.Semantics;

public class ContinueStatementEvaluator : IContinueStatementEvaluator
{
    private IEventEmitter EventEmitter { get; }

    public ContinueStatementEvaluator( IEventEmitter eventEmitter )
    {
        EventEmitter = eventEmitter;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstContinueStatementNode statement )
    {
        if( !statement.HasParent<AstWhileStatementNode>() )
        {
            EventEmitter.Emit(
                statement.AsErrorEvent(
                    CompilerMessageResources.semantic_error_continue_invalid
                )
            );
        }

        return statement.Clone<AstContinueStatementNode>();
    }
}
