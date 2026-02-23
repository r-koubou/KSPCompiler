using System.Linq;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Declarations;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Extensions;
using KSPCompiler.Resources;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Blocks;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.Domain.Compilation.Symbols.Extensions;
using KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData;
using KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData.Extensions;
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
            var astArg = node.ArgumentList.Arguments[ i ];
            var specificArg = specificArguments[ i ];
            var declaredArg = declaredArguments[ i ];

            if( !specificArg.RequiredDeclareOnInit )
            {
                // on init コールバックで宣言が必要ない場合はスキップ
                continue;
            }

            if( !variableSymbols.TrySearchByName( declaredArg.Name, out var variableSymbol ) )
            {
                // on init コールバックで宣言が必要な引数が宣言されていない
                EventEmitter.Emit(
                    node.AsErrorEvent(
                        CompilerMessageResources.semantic_error_declare_callback_arg_declaration_required,
                        node.Name,
                        declaredArg.Name.Value
                    )
                );

                continue;
            }

            // プリミティブ型の型評価
            if( TypeCompatibility.IsTypeCompatible( variableSymbol.DataType, specificArg.DataType ) )
            {
                continue;
            }

            // シンボル定義と実際の変数のUI情報の有無の不一致
            if( specificArg.UITypeNames.Count == 0 && variableSymbol.UIType != UITypeSymbol.Null )
            {
                EventEmitter.Emit(
                    astArg.AsErrorEvent(
                        CompilerMessageResources.semantic_error_declare_callback_arg_incompatible,
                        astArg.Name,
                        specificArg.DataType.ToMessageString(),
                        declaredArg.DataType.ToMessageString()
                    )
                );
                return;
            }

            // UI型情報で評価
            var matchedUiType = ValidateArgumentUiType( specificArg, variableSymbol );

            if( !matchedUiType )
            {
                // UI情報一致
                EventEmitter.Emit(
                    astArg.AsErrorEvent(
                        CompilerMessageResources.semantic_error_declare_callback_arg_ui_incompatible,
                        astArg.Name,
                        string.Join( ", ", specificArg.UITypeNames )
                    )
                );
            }
        }
    }

    private bool ValidateArgumentUiType( CallbackArgumentSymbol specificArg, VariableSymbol declaredVariable )
    {
        var matchedUiType = false;

        var declaredVariableUiType = declaredVariable.UIType;

        foreach( var uiName in specificArg.UITypeNames )
        {
            // コールバック定義側がワイルドカード指定
            if( uiName == UITypeSymbol.AnyUI.Name )
            {
                // 宣言している変数を起点でシンボルテーブルからUI情報取得
                if( !symbolTable.TrySearchUITypeByName( declaredVariable.UIType.Name.Value, out var declaredVarUiTypeSymbol ) )
                {
                    continue;
                }

                // 見つかったUI定義のプリミティブ型と宣言されている変数のプリミティブ型が互換性があるかどうかチェック
                if( TypeCompatibility.IsTypeCompatible( declaredVariable.DataType, declaredVarUiTypeSymbol.DataType ) )
                {
                    matchedUiType = true;
                    break;
                }

                // プリミティブ型が互換性がない場合はワイルドカード指定でも不一致
                matchedUiType = false;
                break;
            }

            // UIの名前が一致するかどうかチェック
            if( uiName != declaredVariableUiType.Name.Value )
            {
                continue;
            }

            // シンボルテーブルからUI情報取得
            if( !symbolTable.TrySearchUITypeByName( uiName, out var uiTypeSymbol ) )
            {
                continue;
            }

            // UIで要求される変数データ型と一致するかどうかチェック
            if( TypeCompatibility.IsTypeCompatible( declaredVariable.DataType, uiTypeSymbol.DataType ) )
            {
                matchedUiType = true;
                break;
            }
        }

        return matchedUiType;
    }
}
