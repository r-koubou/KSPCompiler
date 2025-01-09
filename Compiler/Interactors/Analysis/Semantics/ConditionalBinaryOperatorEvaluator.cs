using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Extensions;
using KSPCompiler.Domain.Events;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;
using KSPCompiler.Interactors.Analysis.Commons.Evaluations;
using KSPCompiler.Interactors.Analysis.Extensions;
using KSPCompiler.Interactors.Analysis.Semantics.Extensions;
using KSPCompiler.Resources;
using KSPCompiler.UseCases.Analysis.Evaluations.Operators;

namespace KSPCompiler.Interactors.Analysis.Semantics;

public class ConditionalBinaryOperatorEvaluator : IConditionalBinaryOperatorEvaluator
{
    private IEventEmitter EventEmitter { get; }

    public ConditionalBinaryOperatorEvaluator( IEventEmitter eventEmitter )
    {
        EventEmitter = eventEmitter;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstExpressionNode expr )
    {
        /*
                    operator
                       +
                       |
                  +----+----+
                  |         |
              expr.Left   expr.Right
        */

        if( expr.ChildNodeCount != 2 || !expr.Id.IsBooleanSupportedBinaryOperator() )
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

        var leftType = evaluatedLeft.TypeFlag;
        var rightType = evaluatedRight.TypeFlag;

        var result = expr.Clone<AstExpressionNode>();
        result.TypeFlag = DataTypeFlag.TypeBool;

        if( DataTypeFlagUtility.AnyFallback( leftType, rightType ) )
        {
            // 評価途中でフォールバックされている場合は型判定をスキップ
        }
        else if( !TypeCompatibility.IsTypeCompatible( leftType, rightType ) )
        {
            EventEmitter.Emit(
                expr.AsErrorEvent(
                    CompilerMessageResources.semantic_error_binaryoprator_compatible,
                    leftType.ToMessageString(),
                    rightType.ToMessageString()
                )
            );

            return result;
        }

        // 値が畳み込みされた値（リテラル値）であれば、式ノードをリテラルに置き換える
        if( evaluatedLeft.IsLiteralNode() )
        {
            expr.Left = evaluatedLeft;
        }
        if( evaluatedRight.IsLiteralNode() )
        {
            expr.Right = evaluatedRight;
        }

        return result;
    }
}
