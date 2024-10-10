using KSPCompiler.Domain.Ast.Analyzers.Convolutions;
using KSPCompiler.Domain.Ast.Analyzers.Evaluators;
using KSPCompiler.Domain.Ast.Extensions;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Extensions;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;
using KSPCompiler.Resources;

namespace KSPCompiler.Domain.Ast.Analyzers.Semantics;

public sealed class NumericUnaryOperatorEvaluator : IUnaryOperatorEvaluator
{
    private IAstVisitor AstVisitor { get; }
    private ICompilerMessageManger CompilerMessageManger { get; }
    private IPrimitiveConvolutionEvaluator<int> IntegerConvolutionEvaluator { get; }
    private IPrimitiveConvolutionEvaluator<double> RealConvolutionEvaluator { get; }

    public NumericUnaryOperatorEvaluator(
        IAstVisitor astVisitor,
        ICompilerMessageManger compilerMessageManger,
        IPrimitiveConvolutionEvaluator<int> integerConvolutionEvaluator,
        IPrimitiveConvolutionEvaluator<double> realConvolutionEvaluator )
    {
        AstVisitor                  = astVisitor;
        CompilerMessageManger       = compilerMessageManger;
        IntegerConvolutionEvaluator = integerConvolutionEvaluator;
        RealConvolutionEvaluator    = realConvolutionEvaluator;
    }

    public IAstNode Evaluate( IAstVisitor<IAstNode> visitor, AstExpressionSyntaxNode expr, AbortTraverseToken abortTraverseToken )
    {
        /*
              <<operator>> expr
                   +
                   |
                expr.Left
        */

        if( expr.ChildNodeCount != 1 || !expr.Id.IsUnaryOperator())
        {
            throw new AstAnalyzeException( expr, "Invalid unary operator" );
        }

        if( expr.Left.Accept( visitor, abortTraverseToken ) is not AstDefaultExpression evaluatedLeft )
        {
            throw new AstAnalyzeException( expr, "Failed to evaluate of unary operator" );
        }

        if( abortTraverseToken.Aborted )
        {
            return AstDefaultExpression.Null;
        }

        var typeEvalResult = EvaluateDataType( expr, evaluatedLeft, out var resultType );

        if( !typeEvalResult )
        {
            CompilerMessageManger.Error(
                expr,
                CompilerMessageResources.semantic_error_unrayoprator_bitnot_compatible,
                evaluatedLeft.TypeFlag.ToMessageString()
            );

            abortTraverseToken.Abort();
            return AstDefaultExpression.Null;
        }

        // リテラル、定数なら畳み込み
        if( TryConvolutionValue( expr, evaluatedLeft, resultType, out var convolutedValue ) )
        {
            return convolutedValue;
        }

        return new AstDefaultExpression( expr )
        {
            TypeFlag = resultType
        };
    }

    private bool TryConvolutionValue( AstExpressionSyntaxNode expr, AstDefaultExpression left, DataTypeFlag resultType, out AstDefaultExpression convolutedValue )
    {
        convolutedValue = default!;

        if( left.Reserved ||
            left.TypeFlag.IsArray() ||
            !left.IsConstant )
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

            convolutedValue = new AstIntLiteral( convolutedInt.Value );
            return true;
        }
        else if( resultType.IsReal() )
        {
            var convolutedReal = RealConvolutionEvaluator.Evaluate( expr, 0 );

            if( convolutedReal == null )
            {
                return false;
            }

            convolutedValue = new AstRealLiteral( convolutedReal.Value );
            return true;
        }

        return false;
    }

    private bool EvaluateDataType( AstExpressionSyntaxNode expr, AstDefaultExpression left, out DataTypeFlag resultType )
    {
        resultType = DataTypeFlag.None;

        // 個別に判定しているのは、KSP が real から int の暗黙の型変換を持っていないため

        if( left.TypeFlag.IsInt() )
        {
            resultType = DataTypeFlag.TypeInt;
            return true;
        }

        // real は unary minus のみで bitwise not は不可
        if( left.TypeFlag.IsReal() && expr.Id == AstNodeId.UnaryMinus)
        {
            resultType = DataTypeFlag.TypeReal;
            return true;
        }

        return false;
    }
}
