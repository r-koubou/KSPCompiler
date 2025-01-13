using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Events;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.Extensions;
using KSPCompiler.Interactors.Analysis.Extensions;
using KSPCompiler.Resources;
using KSPCompiler.UseCases.Analysis.Evaluations.Declarations;

namespace KSPCompiler.Interactors.Analysis.Semantics;

public class CallbackDeclarationEvaluator : ICallbackDeclarationEvaluator
{
    private IEventEmitter EventEmitter { get; }

    private ISymbolTable<CallbackSymbol> BuiltInCallbackSymbols { get; }

    private ISymbolTable<CallbackSymbol> UserCallbackSymbols { get; }

    public CallbackDeclarationEvaluator(
        IEventEmitter eventEmitter,
        ISymbolTable<CallbackSymbol> builtInCallbackSymbols,
        ISymbolTable<CallbackSymbol> userCallbackSymbols )
    {
        EventEmitter           = eventEmitter;
        BuiltInCallbackSymbols = builtInCallbackSymbols;
        UserCallbackSymbols    = userCallbackSymbols;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstCallbackDeclarationNode node )
    {
        CallbackSymbol thisCallback;

        // NI予約済みコールバックの検査
        if( !BuiltInCallbackSymbols.TrySearchByName( node.Name, out var builtInCallback ) )
        {
            EventEmitter.Emit(
                node.AsWarningEvent(
                    CompilerMessageResources.symbol_warning_declare_callback_unkown,
                    node.Name
                )
            );

            // 暫定のシンボル生成
            thisCallback = node.As();
        }
        else
        {
            thisCallback              = builtInCallback.Clone<CallbackSymbol>();
            thisCallback.CommentLines = node.CommentLines;
        }

        if( !UserCallbackSymbols.Add( thisCallback ) )
        {
            EventEmitter.Emit(
                node.AsErrorEvent(
                    CompilerMessageResources.symbol_error_declare_callback_already,
                    node.Name
                )
            );
        }

        thisCallback.DefinedPosition = node.CallbackNamePosition;

        return node;
    }
}
