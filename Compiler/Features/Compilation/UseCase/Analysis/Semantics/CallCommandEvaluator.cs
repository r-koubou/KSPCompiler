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
using KSPCompiler.Shared.EventEmitting;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Semantics;

public class CallCommandEvaluator : ICallCommandEvaluator
{
    private IEventEmitter EventEmitter { get; }

    private AggregateSymbolTable SymbolTable { get; }

    public CallCommandEvaluator( IEventEmitter eventEmitter, AggregateSymbolTable symbolTable )
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

        if( !SymbolTable.TrySearchCommandByName( evaluatedSymbolExpr.Name, out var commandSymbol ) )
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
        var arguments = expr.Right as AstExpressionListNode;
        var callArgs = new List<AstExpressionNode>();

        if( arguments != null )
        {
            callArgs.AddRange( arguments.Expressions );
        }

        // 引数の式を事前に評価し、型情報を取得する
        var evaluatedCallArgs = new List<AstExpressionNode>();

        foreach( var callArg in callArgs )
        {
            if( callArg.Accept( visitor ) is not AstExpressionNode evaluatedArg )
            {
                throw new AstAnalyzeException( callArg, "Failed to evaluate command argument" );
            }

            evaluatedCallArgs.Add( evaluatedArg );
        }

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
            // 引数オーバーロード毎の引数の数が一致しない時点で評価はここまで
            if( symbolArgs.Count != evaluatedCallArgs.Count )
            {
                continue;
            }

            if( ValidateCommandArgumentType( expr, x, evaluatedCallArgs, symbolArgs ) )
            {
                return true;
            }
            #endregion ~With arguments command calling
        }

        // 全てのコマンド引数オーバーロードの評価がパスできなかった場合はエラー
        var commandName = commandSymbols.First().Name;
        EventEmitter.Emit(
            expr.AsErrorEvent(
                CompilerMessageResources.semantic_error_command_arg_incompatible,
                commandName,
                commandSymbols.ToIncompatibleMessage(),
                evaluatedCallArgs.ToIncompatibleMessage( commandName )
            )
        );

        return false;
    }

    private bool ValidateCommandArgumentType( AstCallCommandExpressionNode expr, CommandSymbol commandSymbol, IReadOnlyList<AstExpressionNode> evaluatedCallArgs, IReadOnlyList<CommandArgumentSymbol> symbolArgs )
    {
        for( var i = 0; i < evaluatedCallArgs.Count; i++ )
        {
            var symbolArg = symbolArgs[ i ];
            var evaluatedArg = evaluatedCallArgs[ i ];

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

            // 引数の型が不一致
            return false;
        }

        // 引数が畳み込みでリテラルになっていれば引数の式を置き換える
        ReplaceConvolutedCommandArguments( expr, evaluatedCallArgs );

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
