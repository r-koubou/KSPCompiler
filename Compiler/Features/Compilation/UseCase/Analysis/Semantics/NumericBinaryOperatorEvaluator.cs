using KSPCompiler.Features.Compilation.Domain.Ast.Nodes;
using KSPCompiler.Features.Compilation.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Features.Compilation.Domain.Ast.Nodes.Extensions;
using KSPCompiler.Features.Compilation.Domain.Symbols;
using KSPCompiler.Features.Compilation.Domain.Symbols.MetaData;
using KSPCompiler.Features.Compilation.Domain.Symbols.MetaData.Extensions;
using KSPCompiler.Features.Compilation.UseCase.Abstractions.Evaluations.Convolutions.Integers;
using KSPCompiler.Features.Compilation.UseCase.Abstractions.Evaluations.Convolutions.Reals;
using KSPCompiler.Features.Compilation.UseCase.Abstractions.Evaluations.Operators;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Commons.Evaluations;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Extensions;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Semantics.Extensions;
using KSPCompiler.Shared.EventEmitting;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Semantics;

public class NumericBinaryOperatorEvaluator : IBinaryOperatorEvaluator
{
    protected IEventEmitter EventEmitter { get; }
    protected AggregateSymbolTable SymbolTable { get; }
    protected IIntegerConvolutionEvaluator IntegerConvolutionEvaluator { get; }
    protected IRealConvolutionEvaluator RealConvolutionEvaluator { get; }

    private static AstExpressionNode CreateEvaluateNode( AstExpressionNode source, DataTypeFlag type )
    {
        var result = source.Clone<AstExpressionNode>();
        result.TypeFlag = type;

        return result;
    }

    public NumericBinaryOperatorEvaluator(
        IEventEmitter eventEmitter,
        AggregateSymbolTable symbolTable,
        IIntegerConvolutionEvaluator integerConvolutionEvaluator,
        IRealConvolutionEvaluator realConvolutionEvaluator )
    {
        EventEmitter                = eventEmitter;
        SymbolTable                 = symbolTable;
        IntegerConvolutionEvaluator = integerConvolutionEvaluator;
        RealConvolutionEvaluator    = realConvolutionEvaluator;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstExpressionNode expr )
    {
        /*
                <<operator>> expr
                       +
                       |
                  +----+----+
                  |         |
              expr.Left   expr.Right
        */

        if( expr.ChildNodeCount != 2 || !expr.Id.IsNumericSupportedBinaryOperator())
        {
            throw new AstAnalyzeException( expr, "Invalid binary operator" );
        }

        if( expr.Left.Accept( visitor ) is not AstExpressionNode evaluatedLeft )
        {
            throw new AstAnalyzeException( expr, "Failed to evaluate left side of binary operator" );
        }
        if( expr.Right.Accept( visitor ) is not AstExpressionNode evaluatedRight )
        {
            throw new AstAnalyzeException( expr, "Failed to evaluate right side of binary operator" );
        }

        var typeEvalResult = EvaluateDataType( expr, evaluatedLeft, evaluatedRight, out var resultType );

        if( !typeEvalResult )
        {
            return CreateEvaluateNode(
                expr,
                evaluatedLeft.TypeFlag | evaluatedRight.TypeFlag
            );
        }

        // 評価対象が変数の場合、初期化されているかチェック
        if( !evaluatedLeft.EvaluateSymbolState( expr, EventEmitter, SymbolTable ) ||
            !evaluatedRight.EvaluateSymbolState( expr, EventEmitter, SymbolTable ) )
        {
            return CreateEvaluateNode(
                expr,
                evaluatedLeft.TypeFlag | evaluatedRight.TypeFlag
            );
        }

        // 左辺、右辺共にリテラル、定数なら畳み込み
        if( TryConvolutionValue( visitor, expr, evaluatedLeft, evaluatedRight, resultType, out var convolutedValue ) )
        {
            return convolutedValue;
        }

        return CreateEvaluateNode( expr, resultType );
    }

    private bool TryConvolutionValue( IAstVisitor visitor, AstExpressionNode expr, AstExpressionNode left, AstExpressionNode right, DataTypeFlag resultType, out AstExpressionNode convolutedValue )
    {
        convolutedValue = default!;

        // 変数シンボルで、ビルトイン変数であれば畳み込みしない
        if( left is AstSymbolExpressionNode { BuiltIn: true } )
        {
            return false;
        }
        if( right is AstSymbolExpressionNode { BuiltIn: true } )
        {
            return false;
        }

        // フォールバックされている場合は畳み込みしない
        if( DataTypeFlagUtility.AnyFallback( left.TypeFlag, right.TypeFlag ) )
        {
            return false;
        }

        if( left.TypeFlag.IsArray() || right.TypeFlag.IsArray() ||
            !left.Constant || !right.Constant )
        {
            return false;
        }

        if( resultType.IsInt() )
        {
            var convolutedInt = IntegerConvolutionEvaluator.Evaluate( visitor, expr, 0 );

            if( convolutedInt == null )
            {
                return false;
            }

            convolutedValue = new AstIntLiteralNode( convolutedInt.Value );
            return true;
        }
        else if( resultType.IsReal() )
        {
            var convolutedReal = RealConvolutionEvaluator.Evaluate( visitor, expr, 0 );

            if( convolutedReal == null )
            {
                return false;
            }

            convolutedValue = new AstRealLiteralNode( convolutedReal.Value );
            return true;
        }

        return false;
    }

    private bool EvaluateDataType(
        AstExpressionNode operatorNode,
        AstExpressionNode left,
        AstExpressionNode right,
        out DataTypeFlag resultType )
    {
        resultType = DataTypeFlag.None;

        if( DataTypeFlagUtility.AnyFallback( left.TypeFlag, right.TypeFlag ) )
        {
            // 評価途中でフォールバックされている場合は型判定をスキップ
            resultType = DataTypeFlag.FallBack;
            return true;
        }

        // 個別に判定しているのは、KSP が real から int の暗黙の型変換を持っていないため

        if( left.TypeFlag.IsInt() && right.TypeFlag.IsInt() )
        {
            resultType = DataTypeFlag.TypeInt;
            return true;
        }

        if( left.TypeFlag.IsReal() && right.TypeFlag.IsReal() )
        {
            // Real型はビット演算はサポートしていない
            if( !operatorNode.Id.IsRealSupportedBinaryOperator() )
            {
                EventEmitter.Emit(
                    operatorNode.AsErrorEvent(
                        CompilerMessageResources.semantic_error_binaryoprator_not_supported,
                        left.TypeFlag.ToMessageString(),
                        right.TypeFlag.ToMessageString()
                    )
                );

                return false;
            }

            resultType = DataTypeFlag.TypeReal;
            return true;
        }

        EventEmitter.Emit(
            operatorNode.AsErrorEvent(
                CompilerMessageResources.semantic_error_binaryoprator_compatible,
                left.TypeFlag.ToMessageString(),
                right.TypeFlag.ToMessageString()
            )
        );

        return false;
    }
}
