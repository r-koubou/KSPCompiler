using KSPCompiler.Domain.Ast.Extensions;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Extensions;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;
using KSPCompiler.Interactor.Analysis.Commons.Evaluations;
using KSPCompiler.Resources;
using KSPCompiler.UseCases.Analysis.Evaluations.Convolutions.Integers;
using KSPCompiler.UseCases.Analysis.Evaluations.Convolutions.Reals;
using KSPCompiler.UseCases.Analysis.Evaluations.Operators;

namespace KSPCompiler.Interactor.Analysis.Semantics;

public class NumericBinaryOperatorEvaluator : IBinaryOperatorEvaluator
{
    protected ICompilerMessageManger CompilerMessageManger { get; }
    protected IIntegerConvolutionEvaluator IntegerConvolutionEvaluator { get; }
    protected IRealConvolutionEvaluator RealConvolutionEvaluator { get; }

    private static AstExpressionNode CreateEvaluateNode( AstExpressionNode source, DataTypeFlag type )
    {
        var result = source.Clone<AstExpressionNode>();
        result.TypeFlag = type;

        return result;
    }

    public NumericBinaryOperatorEvaluator(
        ICompilerMessageManger compilerMessageManger,
        IIntegerConvolutionEvaluator integerConvolutionEvaluator,
        IRealConvolutionEvaluator realConvolutionEvaluator )
    {
        CompilerMessageManger       = compilerMessageManger;
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

        if( left.Reserved || right.Reserved ||
            left.TypeFlag.IsArray() || right.TypeFlag.IsArray() ||
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
                CompilerMessageManger.Error(
                    operatorNode,
                    CompilerMessageResources.semantic_error_binaryoprator_not_supported,
                    left.TypeFlag.ToMessageString(),
                    right.TypeFlag.ToMessageString()
                );

                return false;
            }

            resultType = DataTypeFlag.TypeReal;
            return true;
        }

        CompilerMessageManger.Error(
            operatorNode,
            CompilerMessageResources.semantic_error_binaryoprator_compatible,
            left.TypeFlag.ToMessageString(),
            right.TypeFlag.ToMessageString()
        );

        return false;
    }
}
