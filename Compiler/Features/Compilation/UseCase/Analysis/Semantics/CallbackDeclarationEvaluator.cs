using System.Linq;

using KSPCompiler.Features.Compilation.Domain.Ast.Nodes;
using KSPCompiler.Features.Compilation.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Features.Compilation.Domain.Symbols;
using KSPCompiler.Features.Compilation.Domain.Symbols.Extensions;
using KSPCompiler.Features.Compilation.Domain.Symbols.MetaData;
using KSPCompiler.Features.Compilation.UseCase.Abstractions.Evaluations.Declarations;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Extensions;
using KSPCompiler.Shared.EventEmitting;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Semantics;

public class CallbackDeclarationEvaluator(
    IEventEmitter eventEmitter,
    AggregateSymbolTable symbolTable )
    : ICallbackDeclarationEvaluator
{
    private IEventEmitter EventEmitter { get; } = eventEmitter;

    private ICallbackSymbolTable BuiltInCallbackSymbols { get; } = symbolTable.BuiltInCallbacks;

    private ICallbackSymbolTable UserCallbackSymbols { get; } = symbolTable.UserCallbacks;

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
            // 現状同名のコールバックで引数構成が異なる仕様ではないので First() のみ
            thisCallback              = (CallbackSymbol)builtInCallback.First().Clone();
            thisCallback.CommentLines = node.CommentLines;
        }

        // スクリプトで記述している引数シンボル名に置き換える
        thisCallback.Arguments.Clear();

        foreach( var arg in node.ArgumentList.Arguments )
        {
            var argSymbol = new CallbackArgumentSymbol( false )
            {
                Name = arg.Name,
                DataType = DataTypeUtility.GuessFromSymbolName( arg.Name )
            };
            thisCallback.Arguments.Add( argSymbol );
        }

        var addResult = false;

        if( !thisCallback.AllowMultipleDeclaration )
        {
            addResult = UserCallbackSymbols.AddAsNoOverload( thisCallback );
        }
        else
        {
            addResult = UserCallbackSymbols.AddAsOverload( thisCallback, thisCallback.Arguments );
        }

        if( !addResult )
        {
            EventEmitter.Emit(
                node.AsErrorEvent(
                    CompilerMessageResources.symbol_error_declare_callback_already,
                    node.Name
                )
            );
        }

        thisCallback.Range           = node.Position;
        thisCallback.DefinedPosition = node.CallbackNamePosition;

        return node;
    }
}
