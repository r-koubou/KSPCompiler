using System.Collections.Generic;
using System.Linq;

using KSPCompiler.Features.Compilation.Domain.Ast.Nodes;
using KSPCompiler.Features.Compilation.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Features.Compilation.Domain.Ast.Nodes.Extensions;
using KSPCompiler.Features.Compilation.Domain.Symbols;
using KSPCompiler.Features.Compilation.Domain.Symbols.MetaData;
using KSPCompiler.Features.Compilation.Domain.Symbols.MetaData.Extensions;
using KSPCompiler.Features.Compilation.UseCase.Abstractions.Evaluations.Commands;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Commons.Evaluations;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Extensions;
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
        if( !TryGetCommandSymbol( visitor, expr, out var commandSymbol ) )
        {
            // フォールバックに応じるため代替の式を返す
            var alternative = expr.Clone<AstCallCommandExpressionNode>();
            alternative.TypeFlag = DataTypeFlag.FallBack;

            return alternative;
        }

        // フォールバックに応じるため return せずに続行
        ValidateCommandArguments( visitor, expr, commandSymbol );

        var result = expr.Clone<AstCallCommandExpressionNode>();
        result.TypeFlag = commandSymbol.DataType;

        return result;
    }

    private bool TryGetCommandSymbol( IAstVisitor visitor, AstCallCommandExpressionNode expr, out CommandSymbol result )
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

    private bool ValidateCommandArguments( IAstVisitor visitor, AstCallCommandExpressionNode expr, CommandSymbol commandSymbol )
    {
        var symbolArgs = commandSymbol.Arguments.ToList();

        #region No arguments command calling
        if( expr.Right.IsNull() )
        {
            if( symbolArgs.Count == 0 )
            {
                return true;
            }

            EventEmitter.Emit(
                expr.AsErrorEvent(
                    CompilerMessageResources.semantic_error_command_arg_count,
                    commandSymbol.Name
                )
            );

            return false;
        }
        #endregion ~No arguments command calling

        #region With arguments command calling
        if( expr.Right is not AstExpressionListNode arguments )
        {
            throw new AstAnalyzeException( expr, "Failed to evaluate command arguments" );
        }

        var callArgs   = arguments.Expressions.ToList();

        if( symbolArgs.Count != callArgs.Count )
        {
            EventEmitter.Emit(
                expr.AsErrorEvent(
                    CompilerMessageResources.semantic_error_command_arg_count,
                    commandSymbol.Name
                )
            );

            // フォールバックに応じるため return せずに続行
            // return false;
        }

        return ValidateCommandArgumentType( visitor, expr, commandSymbol, callArgs, symbolArgs );
        #endregion ~With arguments command calling
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

    private bool ValidateCommandArgumentState( AstCallCommandExpressionNode expr, IReadOnlyList<AstExpressionNode> callArgs )
    {
        return callArgs.All( arg => arg.EvaluateSymbolState( expr, EventEmitter, SymbolTable ) );
    }
}
