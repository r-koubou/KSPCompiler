using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Integers;
using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Reals;
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

public class NumericBinaryOperatorEvaluator : IBinaryOperatorEvaluator
{
    protected IAstVisitor AstVisitor { get; }
    protected ICompilerMessageManger CompilerMessageManger { get; }
    protected IIntegerConvolutionEvaluator IntegerConvolutionEvaluator { get; }
    protected IRealConvolutionEvaluator RealConvolutionEvaluator { get; }

    public NumericBinaryOperatorEvaluator(
        IAstVisitor astVisitor,
        ICompilerMessageManger compilerMessageManger,
        IIntegerConvolutionEvaluator integerConvolutionEvaluator,
        IRealConvolutionEvaluator realConvolutionEvaluator )
    {
        AstVisitor                  = astVisitor;
        CompilerMessageManger       = compilerMessageManger;
        IntegerConvolutionEvaluator = integerConvolutionEvaluator;
        RealConvolutionEvaluator    = realConvolutionEvaluator;
    }

    public IAstNode Evaluate( IAstVisitor<IAstNode> visitor, AstExpressionNode expr, AbortTraverseToken abortTraverseToken )
    {
        /*
                <<operator>> expr
                       +
                       |
                  +----+----+
                  |         |
              expr.Left   expr.Right
        */

        if( expr.ChildNodeCount != 2 || !expr.Id.IsBinaryOperator())
        {
            throw new AstAnalyzeException( expr, "Invalid binary operator" );
        }

        if( expr.Left.Accept( visitor, abortTraverseToken ) is not AstExpressionNode evaluatedLeft )
        {
            throw new AstAnalyzeException( expr, "Failed to evaluate left side of binary operator" );
        }
        if( expr.Right.Accept( visitor, abortTraverseToken ) is not AstExpressionNode evaluatedRight )
        {
            throw new AstAnalyzeException( expr, "Failed to evaluate right side of binary operator" );
        }

        if( abortTraverseToken.Aborted )
        {
            return NullAstExpressionNode.Instance;
        }

        var typeEvalResult = EvaluateDataType( expr, evaluatedLeft, evaluatedRight, abortTraverseToken, out var resultType );

        if( !typeEvalResult )
        {
            return NullAstExpressionNode.Instance;
        }

        // 左辺、右辺共にリテラル、定数なら畳み込み
        if( TryConvolutionValue( expr, evaluatedLeft, evaluatedRight, resultType, out var convolutedValue ) )
        {
            return convolutedValue;
        }

        return new AstDefaultExpressionNode( expr )
        {
            TypeFlag = resultType
        };
    }

    private bool TryConvolutionValue( AstExpressionNode expr, AstExpressionNode left, AstExpressionNode right, DataTypeFlag resultType, out AstExpressionNode convolutedValue )
    {
        convolutedValue = default!;

        if( left.Reserved || right.Reserved ||
            left.TypeFlag.IsArray() || right.TypeFlag.IsArray() ||
            !left.IsConstant || !right.IsConstant )
        {
            return false;
        }

        if( resultType.IsInt() )
        {
            var convolutedInt = IntegerConvolutionEvaluator.Evaluate( expr, 0 );

            if( convolutedInt == null )
            {
                return false;
            }

            convolutedValue = new AstIntLiteralNode( convolutedInt.Value );
            return true;
        }
        else if( resultType.IsReal() )
        {
            var convolutedReal = RealConvolutionEvaluator.Evaluate( expr, 0 );

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
        AbortTraverseToken abortTraverseToken,
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

                abortTraverseToken.Abort();
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

        abortTraverseToken.Abort();

        return false;
    }
}
