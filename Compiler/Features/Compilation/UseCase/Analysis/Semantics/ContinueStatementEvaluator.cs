using KSPCompiler.Features.Compilation.UseCase.Abstractions.Evaluations.Statements;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Extensions;
using KSPCompiler.Resources;
using KSPCompiler.Shared.Domain.Ast.Nodes;
using KSPCompiler.Shared.Domain.Ast.Nodes.Statements;
using KSPCompiler.Shared.EventEmitting;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Semantics;

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
