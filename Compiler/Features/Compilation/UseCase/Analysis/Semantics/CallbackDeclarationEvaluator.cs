using System.Linq;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Declarations;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Extensions;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Semantics.Extensions;
using KSPCompiler.Resources;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Blocks;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.Domain.Compilation.Symbols.Extensions;
using KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData;
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

        // 引数の評価

        #region Evaluate Arguments
        var declaredArguments = new CallbackArgumentSymbolList();
        var specificArguments = new CallbackArgumentSymbolList();
        specificArguments.AddRange( thisCallback.Arguments );

        // スクリプトで記述している引数シンボル収集
        foreach( var arg in node.ArgumentList.Arguments )
        {
            var argSymbol = new CallbackArgumentSymbol( false )
            {
                Name     = arg.Name,
                DataType = DataTypeUtility.GuessFromSymbolName( arg.Name )
            };
            declaredArguments.Add( argSymbol );
        }

        // 引数の数の検査（不一致でもそのままフォールバック）
        ValidateArgumentCount( node, specificArguments, declaredArguments );
        // 引数の型の検査（不一致でもそのままフォールバック）
        ValidateArgumentType( node, specificArguments, declaredArguments );
        // 引数の宣言の検査（不一致でもそのままフォールバック）
        ValidateArgumentDeclaration( node, specificArguments, declaredArguments );

        // スクリプトで記述している引数シンボル名に置き換える
        thisCallback.Arguments.Clear();
        thisCallback.Arguments.AddRange( declaredArguments );
        #endregion

        // シンボルテーブルへの登録

        #region Add to Symbol Table
        bool addResult;

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
        #endregion

        thisCallback.Range           = node.Position;
        thisCallback.DefinedPosition = node.CallbackNamePosition;

        return node;
    }

    private bool EqualArgumentCount(
        CallbackArgumentSymbolList specificArguments,
        CallbackArgumentSymbolList declaredArguments )
        => specificArguments.Count == declaredArguments.Count;

    private void ValidateArgumentCount(
        AstCallbackDeclarationNode node,
        CallbackArgumentSymbolList specificArguments,
        CallbackArgumentSymbolList declaredArguments )
    {
        if( !EqualArgumentCount( specificArguments, declaredArguments ) )
        {
            EventEmitter.Emit(
                node.AsErrorEvent(
                    CompilerMessageResources.semantic_error_declare_callback_arg_count,
                    node.Name
                )
            );
        }
    }

    private void ValidateArgumentType(
        AstCallbackDeclarationNode node,
        CallbackArgumentSymbolList specificArguments,
        CallbackArgumentSymbolList declaredArguments )
    {
        if( !EqualArgumentCount( specificArguments, declaredArguments ) )
        {
            // 引数の数が異なる場合はスキップ
            return;
        }

        for( var i = 0; i < node.ArgumentList.ArgumentCount; i++ )
        {
            var astArg = node.ArgumentList.Arguments[ i ];
            var callbackArg = specificArguments[ i ];
            var astType = DataTypeUtility.GuessFromSymbolName( astArg.Name );
            var callbackType = callbackArg.DataType;

            if( !TypeCompatibility.IsTypeCompatible( astType, callbackType ) )
            {
                EventEmitter.Emit(
                    astArg.AsErrorEvent(
                        CompilerMessageResources.semantic_error_declare_callback_arg_incompatible,
                        node.Name,
                        specificArguments.ToIncompatibleMessage( node.Name ),
                        declaredArguments.ToIncompatibleMessage( node.Name )
                    )
                );
            }
        }
    }

    private void ValidateArgumentDeclaration(
        AstCallbackDeclarationNode node,
        CallbackArgumentSymbolList specificArguments,
        CallbackArgumentSymbolList declaredArguments )
    {
        if( !EqualArgumentCount( specificArguments, declaredArguments ) )
        {
            // 引数の数が異なる場合はスキップ
            return;
        }

        var count = specificArguments.Count;
        var variableSymbols = symbolTable.UserVariables;

        for( var i = 0; i < count; i++ )
        {
            var specificArg = specificArguments[ i ];
            var declaredArg = declaredArguments[ i ];

            if( !specificArg.RequiredDeclareOnInit )
            {
                // on init コールバックで宣言が必要ない場合はスキップ
                continue;
            }

            if( !variableSymbols.TrySearchByName( declaredArg.Name, out _ ) )
            {
                // on init コールバックで宣言が必要な引数が宣言されていない
                EventEmitter.Emit(
                    node.AsErrorEvent(
                        CompilerMessageResources.semantic_error_declare_callback_arg_declaration_required,
                        node.Name,
                        declaredArg.Name.Value
                    )
                );
            }
        }
    }
}
