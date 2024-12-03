using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Domain.Events;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Interactors.Analysis.Extensions;
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
        if( !UserFunctions.TrySearchByName( statement.Name, out _ ) )
        {
            EventEmitter.Dispatch(
                statement.AsErrorEvent(
                    CompilerMessageResources.semantic_error_userfunction_unknown,
                    statement.Name
                )
            );
        }

        return statement.Clone<AstCallUserFunctionStatementNode>();
    }
}
