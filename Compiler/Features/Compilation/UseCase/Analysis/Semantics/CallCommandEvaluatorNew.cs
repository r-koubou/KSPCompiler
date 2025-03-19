using System.Collections.Generic;
using System.Linq;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Commands;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Commons.Evaluations;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Extensions;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Semantics.Extensions;
using KSPCompiler.Resources;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Extensions;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData;
using KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData.Extensions;
using KSPCompiler.Shared.EventEmitting;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Semantics;

public class CallCommandEvaluatorNew : ICallCommandEvaluator
{
    private IEventEmitter EventEmitter { get; }

    private AggregateSymbolTable SymbolTable { get; }

    public CallCommandEvaluatorNew( IEventEmitter eventEmitter, AggregateSymbolTable symbolTable )
    {
        EventEmitter = eventEmitter;
        SymbolTable  = symbolTable;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstCallCommandExpressionNode expr )
    {
        if( !TryGetCommandSymbol( visitor, expr, out var symbols ) )
        {
            // フォールバックに応じるため代替の式を返す
            var alternative = expr.Clone<AstCallCommandExpressionNode>();
            alternative.TypeFlag = DataTypeFlag.FallBack;

            return alternative;
        }

        // フォールバックに応じるため return せずに続行
        ValidateCommandArguments( visitor, expr, symbols );

        var result = expr.Clone<AstCallCommandExpressionNode>();

        // 引数オーバーロードが複数あっても、戻り値の型は同じ想定
        result.TypeFlag = symbols.First().DataType;

        return result;
    }

    private bool TryGetCommandSymbol( IAstVisitor visitor, AstCallCommandExpressionNode expr, out IReadOnlyCollection<CommandSymbol> result )
    {
        result = default!;

        if( expr.Left.Accept( visitor ) is not AstExpressionNode evaluatedSymbolExpr )
        {
            throw new AstAnalyzeException( expr, "Failed to evaluate command symbol" );
        }

        if( !SymbolTable.TrySearchCommandByNameNew( evaluatedSymbolExpr.Name, out var commandSymbol ) )
        {
            EventEmitter.Emit(
                expr.AsWarningEvent(
                    CompilerMessageResources.semantic_warning_command_unknown,
                    evaluatedSymbolExpr.Name
                )
            );

            return false;
        }

        result = commandSymbol;

        return true;
    }

    private bool ValidateCommandArguments(
        IAstVisitor visitor,
        AstCallCommandExpressionNode expr,
        IReadOnlyCollection<CommandSymbol> commandSymbols )
    {
        foreach( var x in commandSymbols )
        {
            var symbolArgs = x.Arguments.ToList();

            #region No arguments command calling
            if( expr.Right.IsNull() )
            {
                if( symbolArgs.Count == 0 )
                {
                    return true;
                }
            }
            #endregion ~No arguments command calling

            #region With arguments command calling
            if( expr.Right is not AstExpressionListNode arguments )
            {
                throw new AstAnalyzeException( expr, "Failed to evaluate command arguments" );
            }

            var callArgs   = arguments.Expressions.ToList();

            // 引数オーバーロード導入のため、引数の数チェックは行わない
            // if( symbolArgs.Count != callArgs.Count )
            // {
            //     EventEmitter.Emit(
            //         expr.AsErrorEvent(
            //             CompilerMessageResources.semantic_error_command_arg_count,
            //             x.Name
            //         )
            //     );
            //
            //     // フォールバックに応じるため return せずに続行
            //     // return false;
            // }

            if( ValidateCommandArgumentType( visitor, expr, x, callArgs, symbolArgs ) )
            {
                return true;
            }
            #endregion ~With arguments command calling
        }

        // 全てのコマンド引数オーバーロードと一致しなかった場合はエラー

        EventEmitter.Emit(
            expr.AsErrorEvent(
                CompilerMessageResources.semantic_error_command_arg_incompatible,
                commandSymbols.First().Name
            )
        );

        return false;
    }

    private bool ValidateCommandArgumentType( IAstVisitor visitor, AstCallCommandExpressionNode expr, CommandSymbol commandSymbol, IReadOnlyList<AstExpressionNode> callArgs, IReadOnlyList<CommandArgumentSymbol> symbolArgs )
    {
        var evaluatedArgs = new List<AstExpressionNode>();

        for( var i = 0; i < callArgs.Count; i++ )
        {
            var symbolArg = symbolArgs[ i ];
            var callArg = callArgs[ i ];

            if( callArg.Accept( visitor ) is not AstExpressionNode evaluatedArg )
            {
                throw new AstAnalyzeException( callArg, "Failed to evaluate command argument" );
            }

            evaluatedArgs.Add( evaluatedArg );

            // プリミティブ型の型評価
            if( TypeCompatibility.IsTypeCompatible( evaluatedArg.TypeFlag, symbolArg.DataType ) )
            {
                continue;
            }
            else
            {
                // UIチェック

                // UI情報を持つ場合
                var matchedUiType = false;

                foreach( var uiName in symbolArg.UITypeNames )
                {
                    // ワイルドカード指定の場合は合致とみなす
                    if( uiName == UITypeSymbol.AnyUI.Name )
                    {
                        matchedUiType = true;
                        break;
                    }

                    // 個別のUIのチェック

                    if( !SymbolTable.TrySearchUITypeByName( uiName, out var uiTypeSymbol ) )
                    {
                        // UIが見つからない場合は型不一致としてエラー
                        break;
                    }

                    // UIで定義する変数データ型と一致するかどうかチェック
                    if( !TypeCompatibility.IsTypeCompatible( evaluatedArg.TypeFlag, uiTypeSymbol.DataType ) )
                    {
                        continue;
                    }

                    matchedUiType = true;
                    break;
                }

                if( matchedUiType )
                {
                    continue;
                }
            }

            EventEmitter.Emit(
                callArg.AsErrorEvent(
                    CompilerMessageResources.semantic_error_command_arg_incompatible,
                    commandSymbol.Name,
                    i + 1, // 1-based index
                    symbolArg.DataType.ToMessageString(),
                    callArg.TypeFlag.ToMessageString()
                )
            );

            return false;
        }

        // 引数が畳み込みでリテラルになっていれば引数の式を置き換える
        ReplaceConvolutedCommandArguments( expr, evaluatedArgs );

        return true;
    }

    private static void ReplaceConvolutedCommandArguments( AstCallCommandExpressionNode expr, IReadOnlyList<AstExpressionNode> evaluatedArgs )
    {
        if( expr.Right is AstExpressionListNode args )
        {
            args.ReplaceConvolutedExpressions( evaluatedArgs, args );
        }
    }
}
