using System.Collections.Generic;
using System.Linq;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Events;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;
using KSPCompiler.Interactors.Analysis.Commons.Evaluations;
using KSPCompiler.Interactors.Analysis.Extensions;
using KSPCompiler.Interactors.Analysis.Semantics.Extensions;
using KSPCompiler.Resources;
using KSPCompiler.UseCases.Analysis.Evaluations.Commands;

namespace KSPCompiler.Interactors.Analysis.Semantics;

public class CallCommandEvaluator : ICallCommandEvaluator
{
    private IEventEmitter EventEmitter { get; }

    private IVariableSymbolTable Variables { get; }

    private ICommandSymbolTable Commands { get; }

    private IUITypeSymbolTable UITypes { get; }

    public CallCommandEvaluator( IEventEmitter eventEmitter, IVariableSymbolTable variables, ICommandSymbolTable commands, IUITypeSymbolTable uiTypeSymbolTable )
    {
        EventEmitter = eventEmitter;
        Variables    = variables;
        Commands     = commands;
        UITypes      = uiTypeSymbolTable;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstCallCommandExpressionNode expr )
    {
        if( !TryGetCommandSymbol( visitor, expr, out var commandSymbol ) )
        {
            return NullAstExpressionNode.Instance;
        }

        if( !ValidateCommandArguments( visitor, expr, commandSymbol ) )
        {
            return NullAstExpressionNode.Instance;
        }

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

        if( !Commands.TrySearchByName( evaluatedSymbolExpr.Name, out var commandSymbol ) )
        {
            EventEmitter.Dispatch(
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
        if( expr.Right is not AstExpressionListNode arguments )
        {
            throw new AstAnalyzeException( expr, "Failed to evaluate command arguments" );
        }

        var symbolArgs = commandSymbol.Arguments.ToList();
        var callArgs   = arguments.Expressions.ToList();

        if( symbolArgs.Count != callArgs.Count )
        {
            EventEmitter.Dispatch(
                expr.AsErrorEvent(
                    CompilerMessageResources.semantic_error_command_arg_count,
                    commandSymbol.Name
                )
            );

            return false;
        }

        return ValidateCommandArgumentType( visitor, expr, commandSymbol, callArgs, symbolArgs );
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
                if( !symbolArg.UITypeNames.Any() )
                {
                    continue;
                }

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

                    if( UITypes.TrySearchByName( uiName, out _ ) )
                    {
                        matchedUiType = true;
                        break;
                    }
                }

                if( matchedUiType )
                {
                    continue;
                }
            }

            EventEmitter.Dispatch(
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
        return callArgs.All( arg => arg.EvaluateSymbolState( expr, EventEmitter, Variables ) );
    }
}
