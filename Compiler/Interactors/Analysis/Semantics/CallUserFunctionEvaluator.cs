using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Domain.Events;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Interactors.Analysis.Extensions;
using KSPCompiler.Resources;
using KSPCompiler.UseCases.Analysis.Evaluations.UserFunctions;

namespace KSPCompiler.Interactors.Analysis.Semantics;

public class CallUserFunctionEvaluator( IEventEmitter eventEmitter, AggregateSymbolTable symbolTable )
    : ICallUserFunctionEvaluator
{
    private IEventEmitter EventEmitter { get; } = eventEmitter;

    private IUserFunctionSymbolSymbolTable UserFunctions { get; } = symbolTable.UserFunctions;

    public IAstNode Evaluate( IAstVisitor visitor, AstCallUserFunctionStatementNode statement )
    {
        if( statement.TryGetParent<AstCallbackDeclarationNode>( out var callback ) && callback.Name == "init" )
        {
            EventEmitter.Emit(
                statement.AsErrorEvent(
                    CompilerMessageResources.semantic_error_userfunction_call_initcallback,
                    statement.Symbol.Name,
                    callback.Name
                )
            );
        }
        else if( !UserFunctions.TrySearchByName( statement.Symbol.Name, out _ ) )
        {
            EventEmitter.Emit(
                statement.AsErrorEvent(
                    CompilerMessageResources.semantic_error_userfunction_unknown,
                    statement.Symbol.Name
                )
            );
        }

        return statement.Clone<AstCallUserFunctionStatementNode>();
    }
}
