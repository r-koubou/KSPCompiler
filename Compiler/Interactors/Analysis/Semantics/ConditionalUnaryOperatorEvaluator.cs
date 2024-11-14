using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Extensions;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;
using KSPCompiler.Interactors.Analysis.Commons.Evaluations;
using KSPCompiler.Interactors.Analysis.Commons.Extensions;
using KSPCompiler.Resources;
using KSPCompiler.UseCases.Analysis.Evaluations.Convolutions.Booleans;
using KSPCompiler.UseCases.Analysis.Evaluations.Operators;

namespace KSPCompiler.Interactors.Analysis.Semantics;

public class ConditionalUnaryOperatorEvaluator : IConditionalUnaryOperatorEvaluator
{
    private ICompilerMessageManger CompilerMessageManger { get; }
    private IBooleanConvolutionEvaluator BooleanConvolutionEvaluator { get; }

    private static AstExpressionNode CreateEvaluateNode( AstExpressionNode source, DataTypeFlag type )
    {
        var result = source.Clone<AstExpressionNode>();
        result.TypeFlag = type;

        return result;
    }

    public ConditionalUnaryOperatorEvaluator(
        ICompilerMessageManger compilerMessageManger,
        IBooleanConvolutionEvaluator booleanConvolutionEvaluator )
    {
        CompilerMessageManger       = compilerMessageManger;
        BooleanConvolutionEvaluator = booleanConvolutionEvaluator;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstExpressionNode expr )
    {
        /*
          <<logical unary not operator>>
                      expr
                       +
                       |
          <<conditional binary operator>>
                    expr.Left
        */

        if( expr.ChildNodeCount != 1 || !expr.Id.IsBooleanSupportedUnaryOperator())
        {
            throw new AstAnalyzeException( expr, "Expected a unary logical not expression" );
        }

        if( expr.Left.Accept( visitor ) is not AstExpressionNode evaluatedLeft )
        {
            throw new AstAnalyzeException( expr, "Failed to evaluate left side of the unary logical not expression" );
        }

        if( !evaluatedLeft.TypeFlag.IsBoolean() )
        {
            CompilerMessageManger.Error(
                expr,
                CompilerMessageResources.semantic_error_unaryoprator_logicalnot_incompatible,
                evaluatedLeft.TypeFlag.ToMessageString()
            );

            // 上位のノードで評価を継続させるので代替のノードは生成しない
        }

        // リテラルに畳み込み可能なら畳み込む
        if( TryConvolutionValue( visitor, expr, out var convolutedValue ) )
        {
            return convolutedValue;
        }

        return CreateEvaluateNode( expr, DataTypeFlag.TypeBool );
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
