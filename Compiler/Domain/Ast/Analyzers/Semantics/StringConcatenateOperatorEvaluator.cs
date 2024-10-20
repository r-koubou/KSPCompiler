using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Strings;
using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Operators;
using KSPCompiler.Domain.Ast.Extensions;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;
using KSPCompiler.Resources;

namespace KSPCompiler.Domain.Ast.Analyzers.Semantics;

public sealed class StringConcatenateOperatorEvaluator : IStringConcatenateOperatorEvaluator
{
    private IAstVisitor AstVisitor { get; }
    private ICompilerMessageManger CompilerMessageManger { get; }
    private IStringConvolutionEvaluator StringConvolutionEvaluator { get; }

    private static AstExpressionNode CreateEvaluateNode( AstExpressionNode source, DataTypeFlag type )
        => new AstDefaultExpressionNode( source.Id )
        {
            Parent   = source.Parent,
            Name     = source.Name,
            Left     = source.Left,
            Right    = source.Right,
            TypeFlag = type
        };

    public StringConcatenateOperatorEvaluator(
        IAstVisitor astVisitor,
        ICompilerMessageManger compilerMessageManger,
        IStringConvolutionEvaluator stringConvolutionEvaluator )
    {
        AstVisitor                 = astVisitor;
        CompilerMessageManger      = compilerMessageManger;
        StringConvolutionEvaluator = stringConvolutionEvaluator;
    }

    public IAstNode Evaluate( IAstVisitor<IAstNode> visitor, AstExpressionNode expr )
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
        if( TryConvolutionValue( expr, evaluatedLeft, evaluatedRight, out var convolutedValue ) )
        {
            return convolutedValue;
        }

        return CreateEvaluateNode( expr, DataTypeFlag.TypeString );
    }

    private bool TryConvolutionValue( AstExpressionNode expr, AstExpressionNode left, AstExpressionNode right, out AstExpressionNode convolutedNode )
    {
        convolutedNode = default!;

        if( left.TypeFlag.IsArray() || right.TypeFlag.IsArray() ||
            !left.Constant || !right.Constant )
        {
            return false;
        }

        var convolutedValie = StringConvolutionEvaluator.Evaluate( expr, "" );
        if( convolutedValie == null )
        {
            return false;
        }

        convolutedNode = new AstStringLiteralNode( convolutedValie );
        return true;
    }
}
