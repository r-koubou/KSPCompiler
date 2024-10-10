using KSPCompiler.Domain.Ast.Analyzers.Convolutions;
using KSPCompiler.Domain.Ast.Analyzers.Evaluators;
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
    private IObjectConvolutionEvaluator<string> StringConvolutionEvaluator { get; }

    public StringConcatenateOperatorEvaluator(
        IAstVisitor astVisitor,
        ICompilerMessageManger compilerMessageManger,
        IObjectConvolutionEvaluator<string> stringConvolutionEvaluator )
    {
        AstVisitor                 = astVisitor;
        CompilerMessageManger      = compilerMessageManger;
        StringConvolutionEvaluator = stringConvolutionEvaluator;
    }

    public IAstNode Evaluate( IAstVisitor<IAstNode> visitor, AstExpressionNode expr, AbortTraverseToken abortTraverseToken )
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
        if( !expr.HasParent<AstVariableInitializerNode>() )
        {
            CompilerMessageManger.Error(
                expr,
                CompilerMessageResources.semantic_error_variable_invalid_string_initializer
            );

            abortTraverseToken.Abort();
            return NullAstExpressionNode.Instance;
        }

        if( expr.ChildNodeCount != 2 || expr.Id != AstNodeId.StringConcatenate)
        {
            throw new AstAnalyzeException( expr, "Invalid string concatenate operator '&'" );
        }

        if( expr.Left.Accept( visitor, abortTraverseToken ) is not AstExpressionNode evaluatedLeft )
        {
            throw new AstAnalyzeException( expr, "Failed to evaluate left side of concatenate operator '&'" );
        }
        if( expr.Right.Accept( visitor, abortTraverseToken ) is not AstExpressionNode evaluatedRight )
        {
            throw new AstAnalyzeException( expr, "Failed to evaluate right side of concatenate operator '&'" );
        }

        if( abortTraverseToken.Aborted )
        {
            return NullAstExpressionNode.Instance;
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

            abortTraverseToken.Abort();
            return NullAstExpressionNode.Instance;
        }

        // 畳み込み

        //evaluatedLeft.IsConstant

        return new AstDefaultExpressionNode( expr )
        {
            TypeFlag = DataTypeFlag.TypeString
        };
    }
}
