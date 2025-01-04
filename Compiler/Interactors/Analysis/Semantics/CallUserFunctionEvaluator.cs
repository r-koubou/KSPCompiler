using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Domain.Events;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Interactors.Analysis.Extensions;
using KSPCompiler.Resources;
using KSPCompiler.Resources;
using KSPCompiler.UseCases.Analysis.Evaluations.UserFunctions;

namespace KSPCompiler.Interactors.Analysis.Semantics;

public class CallUserFunctionEvaluator : ICallUserFunctionEvaluator
{
    private IEventEmitter EventEmitter { get; }

    private IUserFunctionSymbolSymbolTable UserFunctions { get; }

    public CallUserFunctionEvaluator( IEventEmitter eventEmitter, IUserFunctionSymbolSymbolTable symbolTable )
    {
        EventEmitter  = eventEmitter;
        UserFunctions = symbolTable;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstCallUserFunctionStatementNode statement )
    {
        if( statement.TryGetParent<AstCallbackDeclarationNode>( out var callback ) && callback.Name == "init" )
        {
            EventEmitter.Emit(
                statement.AsErrorEvent(
                    CompilerMessageResources.semantic_error_userfunction_call_initcallback,
                    statement.Name,
                    callback.Name
                )
            );
        }
        else if( !UserFunctions.TrySearchByName( statement.Name, out _ ) )
        {
            EventEmitter.Emit(
                statement.AsErrorEvent(
                    CompilerMessageResources.semantic_error_userfunction_unknown,
                    statement.Name
                )
            );
        }

        return statement.Clone<AstCallUserFunctionStatementNode>();
    }
}
