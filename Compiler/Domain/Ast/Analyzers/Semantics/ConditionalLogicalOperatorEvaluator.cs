using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Booleans;
using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Operators;
using KSPCompiler.Domain.Ast.Extensions;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Extensions;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;
using KSPCompiler.Resources;

namespace KSPCompiler.Domain.Ast.Analyzers.Semantics;

public class ConditionalLogicalOperatorEvaluator : IConditionalLogicalOperatorEvaluator
{
    private ICompilerMessageManger CompilerMessageManger { get; }
    private IBooleanConvolutionEvaluator BooleanConvolutionEvaluator { get; }

    public ConditionalLogicalOperatorEvaluator(
        ICompilerMessageManger compilerMessageManger,
        IBooleanConvolutionEvaluator booleanConvolutionEvaluator )
    {
        CompilerMessageManger       = compilerMessageManger;
        BooleanConvolutionEvaluator = booleanConvolutionEvaluator;
    }

    public IAstNode Evaluate( IAstVisitor<IAstNode> visitor, AstExpressionNode expr )
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
            CompilerMessageManger.Error(
                expr,
                CompilerMessageResources.semantic_error_logicaloprator_incompatible,
                leftType.ToMessageString(),
                rightType.ToMessageString()
            );

            // 上位のノードで評価を継続させるので代替のノードは生成しない
        }

        // リテラルに畳み込み可能なら畳み込む
        if( TryConvolutionValue( expr, out var convolutedValue ) )
        {
            return convolutedValue;
        }

        var result = expr.Clone<AstExpressionNode>();
        result.TypeFlag = DataTypeFlag.TypeBool;

        return result;
    }

    private bool TryConvolutionValue( AstExpressionNode expr, out AstExpressionNode convolutedValue )
    {
        convolutedValue = NullAstExpressionNode.Instance;

        var result = BooleanConvolutionEvaluator.Evaluate( expr, false );

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
