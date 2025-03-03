using KSPCompiler.Features.Compilation.Domain.Ast.Nodes;
using KSPCompiler.Features.Compilation.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Features.Compilation.Domain.Symbols;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Extensions;
using KSPCompiler.Shared.EventEmitting;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Semantics.Extensions;

public static class AstExpressionNodeExtension
{
    /// <summary>
    /// Evaluate the symbol state is initialized or not if the expression is a <see cref="AstSymbolExpressionNode"/>.
    /// </summary>
    /// <returns>true if the symbol state is initialized or node is not a <see cref="AstSymbolExpressionNode"/>. Otherwise, false.</returns>
    public static bool EvaluateSymbolState( this AstExpressionNode self, IAstNode parent, IEventEmitter eventEmitter, AggregateSymbolTable symbolTable )
    {
        if( self is not AstSymbolExpressionNode symbolNode )
        {
            return true;
        }

        if( !symbolTable.TrySearchVariableByName( symbolNode.Name, out var variable ) )
        {
            return true;
        }

        if( variable.State == SymbolState.UnInitialized )
        {
            eventEmitter.Emit(
                parent.AsErrorEvent(
                    CompilerMessageResources.semantic_error_variable_uninitialized,
                    symbolNode.Name
                )
            );

            return false;
        }

        variable.State = SymbolState.Loaded;

        return true;
    }
}
