using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.UserFunctions;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Extensions;
using KSPCompiler.Resources;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Blocks;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Statements;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.EventEmitting;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Semantics;

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
