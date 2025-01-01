using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Events;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;
using KSPCompiler.Interactors.Analysis.Commons.Evaluations;
using KSPCompiler.Interactors.Analysis.Extensions;
using KSPCompiler.Resources;
using KSPCompiler.UseCases.Analysis.Evaluations.Symbols;

namespace KSPCompiler.Interactors.Analysis.Semantics;

public class ArrayElementEvaluator : IArrayElementEvaluator
{
    private IEventEmitter EventEmitter { get; }

    private AggregateSymbolTable SymbolTable { get; }

    public ArrayElementEvaluator( IEventEmitter eventEmitter, AggregateSymbolTable symbolTable )
    {
        EventEmitter = eventEmitter;
        SymbolTable  = symbolTable;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstArrayElementExpressionNode expr )
    {
        /*
                       expr <-- here
                         +
                         |
                    +----+----+
                    |         |
              Left: symbol   Right: index
        */

        if( expr.Left.Accept( visitor ) is not AstSymbolExpressionNode evaluatedLeft )
        {
            throw new AstAnalyzeException( expr, "Failed to evaluate left side of array element" );
        }

        if( expr.Right.Accept( visitor ) is not AstExpressionNode evaluatedRight )
        {
            throw new AstAnalyzeException( expr, "Failed to evaluate right side of array element" );
        }

        if( !SymbolTable.TrySearchVariableByName( evaluatedLeft.Name, out var variableSymbol ) )
        {
            throw new AstAnalyzeException( expr, "Failed to find variable symbol" );
        }

        if( !evaluatedRight.TypeFlag.IsInt() )
        {
            EventEmitter.Emit(
                expr.AsErrorEvent(
                    CompilerMessageResources.semantic_error_array_subscript_compatible,
                    evaluatedRight.TypeFlag.ToMessageString()
                )
            );

            return NullAstExpressionNode.Instance;
        }

        // 添字アクセスの評価
        if( !EvaluateArrayIndex( expr, evaluatedLeft, evaluatedRight, variableSymbol ) )
        {
            return NullAstExpressionNode.Instance;
        }

        // 要素アクセス結果になるので評価結果から配列フラグを削除
        evaluatedLeft.TypeFlag &= ~DataTypeFlag.AttributeArray;

        return evaluatedLeft;
    }

    private bool EvaluateArrayIndex( AstArrayElementExpressionNode expr,  AstExpressionNode evaluatedLeft, AstExpressionNode evaluatedRight, VariableSymbol variableSymbol )
    {
        // ビルトイン変数は添字アクセスの評価は省略
        if( variableSymbol.BuiltIn )
        {
            return true;
        }

        if( evaluatedRight is AstIntLiteralNode intLiteral )
        {
            var arraySize = variableSymbol.ArraySize;

            if( intLiteral.Value < 0 || intLiteral.Value >= arraySize )
            {
                EventEmitter.Emit(
                    expr.AsErrorEvent(
                        CompilerMessageResources.semantic_error_variable_array_outofbounds,
                        arraySize,
                        intLiteral.Value
                    )
                );

                return false;
            }
        }

        return true;
    }
}
