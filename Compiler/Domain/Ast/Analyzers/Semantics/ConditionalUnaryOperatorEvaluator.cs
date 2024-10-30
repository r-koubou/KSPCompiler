using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Operators;
using KSPCompiler.Domain.Ast.Extensions;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Extensions;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;
using KSPCompiler.Resources;

namespace KSPCompiler.Domain.Ast.Analyzers.Semantics;

public class ConditionalUnaryOperatorEvaluator : IUnaryOperatorEvaluator
{
    private ICompilerMessageManger CompilerMessageManger { get; }

    private static AstExpressionNode CreateEvaluateNode( AstExpressionNode source, DataTypeFlag type )
    {
        var result = source.Clone<AstExpressionNode>();
        result.TypeFlag = type;

        return result;
    }

    public ConditionalUnaryOperatorEvaluator( ICompilerMessageManger compilerMessageManger )
        => CompilerMessageManger = compilerMessageManger;

    public IAstNode Evaluate( IAstVisitor<IAstNode> visitor, AstExpressionNode expr )
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
                CompilerMessageResources.semantic_error_unrayoprator_logicalnot_incompatible,
                evaluatedLeft.TypeFlag.ToMessageString()
            );

            // 上位のノードで評価を継続させるので代替のノードは生成しない
        }

        return CreateEvaluateNode( expr, DataTypeFlag.TypeBool );
    }
}
