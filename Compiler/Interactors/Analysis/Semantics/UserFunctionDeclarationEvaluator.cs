using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Events;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.Extensions;
using KSPCompiler.Interactors.Analysis.Extensions;
using KSPCompiler.Resources;
using KSPCompiler.UseCases.Analysis.Evaluations.Declarations;

namespace KSPCompiler.Interactors.Analysis.Semantics;

public class UserFunctionDeclarationEvaluator(
    IEventEmitter eventEmitter,
    AggregateSymbolTable symbolTable )
    : IUserFunctionDeclarationEvaluator
{
    private IEventEmitter EventEmitter { get; } = eventEmitter;
    private ISymbolTable<UserFunctionSymbol> SymbolTable { get; } = symbolTable.UserFunctions;

    public IAstNode Evaluate( IAstVisitor visitor, AstUserFunctionDeclarationNode node )
    {
        var thisUserFunction = node.As();

        if( !SymbolTable.Add( thisUserFunction ) )
        {
            EventEmitter.Emit(
                node.AsErrorEvent(
                    CompilerMessageResources.symbol_error_declare_userfunction_already,
                    node.Name
                )
            );
        }

        thisUserFunction.DefinedPosition = node.FunctionNamePosition;

        return node;
    }
}
