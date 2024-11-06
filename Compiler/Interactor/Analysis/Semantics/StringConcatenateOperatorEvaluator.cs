using KSPCompiler.Domain.Ast.Extensions;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;
using KSPCompiler.Interactor.Analysis.Commons.Evaluations;
using KSPCompiler.Resources;
using KSPCompiler.UseCases.Analysis.Evaluations.Convolutions.Strings;
using KSPCompiler.UseCases.Analysis.Evaluations.Operators;

namespace KSPCompiler.Interactor.Analysis.Semantics;

public sealed class StringConcatenateOperatorEvaluator : IStringConcatenateOperatorEvaluator
{
    private ICompilerMessageManger CompilerMessageManger { get; }
    private IStringConvolutionEvaluator StringConvolutionEvaluator { get; }

    private static AstExpressionNode CreateEvaluateNode( AstExpressionNode source, DataTypeFlag type )
    {
        var result = source.Clone<AstExpressionNode>();
        result.TypeFlag = type;

        return result;
    }

    public StringConcatenateOperatorEvaluator(
        ICompilerMessageManger compilerMessageManger,
        IStringConvolutionEvaluator stringConvolutionEvaluator )
    {
        CompilerMessageManger      = compilerMessageManger;
        StringConvolutionEvaluator = stringConvolutionEvaluator;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstExpressionNode expr )
    {
        /*
               <<operator &>> expr
                       +
                       |
                  +----+----+
                  |         |
              expr.Left   expr.Right
        */

        //--------------------------------------------------------------------------
        // 初期値代入式では使用できない
        //--------------------------------------------------------------------------
        if( expr.HasParent<AstVariableInitializerNode>() )
        {
            CompilerMessageManger.Error(
                expr,
                CompilerMessageResources.semantic_error_variable_invalid_string_initializer
            );

            return CreateEvaluateNode( expr, DataTypeFlag.TypeString );
        }

        if( expr.ChildNodeCount != 2 || expr.Id != AstNodeId.StringConcatenate)
        {
            throw new AstAnalyzeException( expr, "Invalid string concatenate operator '&'" );
        }

        if( expr.Left.Accept( visitor ) is not AstExpressionNode evaluatedLeft )
        {
            throw new AstAnalyzeException( expr, "Failed to evaluate left side of concatenate operator '&'" );
        }
        if( expr.Right.Accept( visitor ) is not AstExpressionNode evaluatedRight )
        {
            throw new AstAnalyzeException( expr, "Failed to evaluate right side of concatenate operator '&'" );
        }

        //----------------------------------------------------------------------
        // KONTAKT内では暗黙の型変換が作動し、文字列型となる
        //----------------------------------------------------------------------

        // BOOL（条件式）は不可
        if( evaluatedLeft.TypeFlag.IsBoolean() || evaluatedRight.TypeFlag.IsBoolean() )
        {
            CompilerMessageManger.Error(
                expr,
                CompilerMessageResources.semantic_error_string_operator_conditional
            );

            return CreateEvaluateNode( expr, DataTypeFlag.TypeBool );
        }

        // 左辺、右辺共にリテラル、定数なら畳み込み
        if( TryConvolutionValue( visitor, expr, evaluatedLeft, evaluatedRight, out var convolutedValue ) )
        {
            return convolutedValue;
        }

        return CreateEvaluateNode( expr, DataTypeFlag.TypeString );
    }

    private bool TryConvolutionValue( IAstVisitor visitor, AstExpressionNode expr, AstExpressionNode left, AstExpressionNode right, out AstExpressionNode convolutedNode )
    {
        convolutedNode = default!;

        if( left.TypeFlag.IsArray() || right.TypeFlag.IsArray() ||
            !left.Constant || !right.Constant )
        {
            return false;
        }

        var convolutedValie = StringConvolutionEvaluator.Evaluate( visitor, expr, "" );
        if( convolutedValie == null )
        {
            return false;
        }

        convolutedNode = new AstStringLiteralNode( convolutedValie );
        return true;
    }
}
