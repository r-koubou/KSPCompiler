using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Extensions;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;
using KSPCompiler.Interactors.Analysis.Commons.Evaluations;
using KSPCompiler.Interactors.Analysis.Commons.Extensions;
using KSPCompiler.Interactors.Analysis.Semantics.Extensions;
using KSPCompiler.Resources;
using KSPCompiler.UseCases.Analysis.Evaluations.Convolutions.Integers;
using KSPCompiler.UseCases.Analysis.Evaluations.Convolutions.Reals;
using KSPCompiler.UseCases.Analysis.Evaluations.Operators;

namespace KSPCompiler.Interactors.Analysis.Semantics;

public sealed class NumericUnaryOperatorEvaluator : IUnaryOperatorEvaluator
{
    private ICompilerMessageManger CompilerMessageManger { get; }
    private IVariableSymbolTable Variables { get; }
    private IIntegerConvolutionEvaluator IntegerConvolutionEvaluator { get; }
    private IRealConvolutionEvaluator RealConvolutionEvaluator { get; }


    private static AstExpressionNode CreateEvaluateNode( AstExpressionNode source, DataTypeFlag type )
    {
        var result = source.Clone<AstExpressionNode>();
        result.TypeFlag = type;

        return result;
    }

    public NumericUnaryOperatorEvaluator(
        ICompilerMessageManger compilerMessageManger,
        IVariableSymbolTable variables,
        IIntegerConvolutionEvaluator integerConvolutionEvaluator,
        IRealConvolutionEvaluator realConvolutionEvaluator )
    {
        CompilerMessageManger       = compilerMessageManger;
        Variables                   = variables;
        IntegerConvolutionEvaluator = integerConvolutionEvaluator;
        RealConvolutionEvaluator    = realConvolutionEvaluator;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstExpressionNode expr )
    {
        /*
              <<operator>> expr
                   +
                   |
                expr.Left
        */

        if( expr.ChildNodeCount != 1 || !expr.Id.IsNumericSupportedUnaryOperator())
        {
            throw new AstAnalyzeException( expr, "Invalid unary operator" );
        }

        if( expr.Left.Accept( visitor ) is not AstExpressionNode evaluatedLeft )
        {
            throw new AstAnalyzeException( expr, "Failed to evaluate of unary operator" );
        }

        var typeEvalResult = EvaluateDataType( expr, evaluatedLeft, out var resultType );

        if( !typeEvalResult )
        {
            CompilerMessageManger.Error(
                expr,
                CompilerMessageResources.semantic_error_unaryoprator_bitnot_compatible,
                evaluatedLeft.TypeFlag.ToMessageString()
            );

            return CreateEvaluateNode( expr, evaluatedLeft.TypeFlag );
        }

        // 評価対象が変数の場合、初期化されているかチェック
        if( !evaluatedLeft.EvaluateSymbolState( expr, CompilerMessageManger, Variables ) )
        {
            return CreateEvaluateNode( expr, evaluatedLeft.TypeFlag );
        }

        // リテラル、定数なら畳み込み
        if( TryConvolutionValue( visitor, expr, evaluatedLeft, resultType, out var convolutedValue ) )
        {
            return convolutedValue;
        }

        return CreateEvaluateNode( expr, resultType );
    }

    private bool TryConvolutionValue( IAstVisitor visitor, AstExpressionNode expr, AstExpressionNode left, DataTypeFlag resultType, out AstExpressionNode convolutedValue )
    {
        convolutedValue = default!;

        // 変数シンボルで、ビルトイン変数であれば畳み込みしない
        if( left is AstSymbolExpressionNode { BuiltIn: true } )
        {
            return false;
        }

        if( left.TypeFlag.IsArray() ||
            !left.Constant )
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

    private bool EvaluateDataType( AstExpressionNode expr, AstExpressionNode left, out DataTypeFlag resultType )
    {
        resultType = DataTypeFlag.None;

        // 個別に判定しているのは、KSP が real から int の暗黙の型変換を持っていないため

        if( left.TypeFlag.IsInt() )
        {
            resultType = DataTypeFlag.TypeInt;
            return true;
        }

        if( left.TypeFlag.IsReal() )
        {
            // Real型は unary minus のみで bitwise not は不可
            if( !expr.Id.IsRealSupportedUnaryOperator() )
            {
                return false;
            }

            resultType = DataTypeFlag.TypeReal;
            return true;
        }

        return false;
    }
}
