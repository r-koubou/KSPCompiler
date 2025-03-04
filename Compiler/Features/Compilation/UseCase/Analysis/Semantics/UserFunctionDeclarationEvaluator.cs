using KSPCompiler.Features.Compilation.UseCase.Abstractions.Evaluations.Declarations;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Extensions;
using KSPCompiler.Resources;
using KSPCompiler.Shared.Domain.Ast.Nodes;
using KSPCompiler.Shared.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Shared.Domain.Symbols;
using KSPCompiler.Shared.Domain.Symbols.Extensions;
using KSPCompiler.Shared.EventEmitting;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Semantics;

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

        thisUserFunction.Range           = node.Position;
        thisUserFunction.DefinedPosition = node.FunctionNamePosition;

        return node;
    }
}
