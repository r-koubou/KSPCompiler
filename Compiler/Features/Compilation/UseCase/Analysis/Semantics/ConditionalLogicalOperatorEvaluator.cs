using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Convolutions.Booleans;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Operators;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Commons.Evaluations;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Extensions;
using KSPCompiler.Resources;
using KSPCompiler.Shared.Domain.Ast.Nodes;
using KSPCompiler.Shared.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Shared.Domain.Ast.Nodes.Extensions;
using KSPCompiler.Shared.Domain.Symbols.MetaData;
using KSPCompiler.Shared.Domain.Symbols.MetaData.Extensions;
using KSPCompiler.Shared.EventEmitting;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Semantics;

public class ConditionalLogicalOperatorEvaluator : IConditionalLogicalOperatorEvaluator
{
    private IEventEmitter EventEmitter { get; }

    private IBooleanConvolutionEvaluator BooleanConvolutionEvaluator { get; }

    public ConditionalLogicalOperatorEvaluator(
        IEventEmitter eventEmitter,
        IBooleanConvolutionEvaluator booleanConvolutionEvaluator )
    {
        EventEmitter                = eventEmitter;
        BooleanConvolutionEvaluator = booleanConvolutionEvaluator;
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

        if( expr.ChildNodeCount != 2 || !expr.Id.IsConditionalLogicalOperator() )
        {
            throw new AstAnalyzeException( expr, "Invalid conditional logical operator" );
        }

        if( expr.Left.Accept( visitor ) is not AstExpressionNode evaluatedLeft )
        {
            throw new AstAnalyzeException( expr, "Failed to evaluate left side of conditional logical operator" );
        }

        if( expr.Right.Accept( visitor ) is not AstExpressionNode evaluatedRight )
        {
            throw new AstAnalyzeException( expr, "Failed to evaluate right side of conditional logical operator" );
        }

        var leftType = evaluatedLeft.TypeFlag;
        var rightType = evaluatedRight.TypeFlag;

        if( !leftType.IsBoolean() || !rightType.IsBoolean() )
        {
            EventEmitter.Emit(
                expr.AsErrorEvent(
                    CompilerMessageResources.semantic_error_logicaloprator_incompatible,
                    leftType.ToMessageString(),
                    rightType.ToMessageString()
                )
            );

            // 上位のノードで評価を継続させるので代替のノードは生成しない
        }

        // リテラルに畳み込み可能なら畳み込む
        if( TryConvolutionValue( visitor, expr, out var convolutedValue ) )
        {
            return convolutedValue;
        }

        var result = expr.Clone<AstExpressionNode>();
        result.TypeFlag = DataTypeFlag.TypeBool;

        return result;
    }

    private bool TryConvolutionValue( IAstVisitor visitor, AstExpressionNode expr, out AstExpressionNode convolutedValue )
    {
        convolutedValue = NullAstExpressionNode.Instance;

        var result = BooleanConvolutionEvaluator.Evaluate( visitor, expr, false );

        if( result == null )
        {
            return false;
        }

        convolutedValue = new AstBooleanLiteralNode( result.Value )
        {
            Parent = expr
        };

        return true;
    }
}
