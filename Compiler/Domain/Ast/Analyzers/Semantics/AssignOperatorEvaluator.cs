using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Operators;
using KSPCompiler.Domain.Ast.Extensions;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;
using KSPCompiler.Resources;

namespace KSPCompiler.Domain.Ast.Analyzers.Semantics;

public sealed class AssignOperatorEvaluator : IAssignOperatorEvaluator
{
    private ICompilerMessageManger CompilerMessageManger { get; }

    private static AstExpressionNode CreateEvaluateNode( AstExpressionNode source, DataTypeFlag type )
        => new AstDefaultExpressionNode( source.Id )
        {
            Parent   = source.Parent,
            Name     = source.Name,
            Left     = source.Left,
            Right    = source.Right,
            TypeFlag = type
        };

    public AssignOperatorEvaluator( ICompilerMessageManger compilerMessageManger )
    {
        CompilerMessageManger = compilerMessageManger;
    }

    public IAstNode Evaluate( IAstVisitor<IAstNode> visitor, AstExpressionNode expr )
    {
        /*
                     := expr
                       +
                       |
                  +----+----+
                  |         |
              expr.Left   expr.Right
              (variable)    (value)
        */

        if( expr.Left.Accept( visitor ) is not AstExpressionNode evaluatedLeft )
        {
            throw new AstAnalyzeException( expr, "Failed to evaluate left side of binary operator" );
        }

        if( expr.Right.Accept( visitor ) is not AstExpressionNode evaluatedRight )
        {
            throw new AstAnalyzeException( expr, "Failed to evaluate right side of binary operator" );
        }

        // 定数には代入できない
        if( evaluatedLeft.Constant )
        {
            CompilerMessageManger.Error(
                expr,
                CompilerMessageResources.semantic_error_assign_to_constant,
                evaluatedLeft.Name
            );

            return CreateEvaluateNode( evaluatedLeft, evaluatedLeft.TypeFlag );
        }

        var leftType  = evaluatedLeft.TypeFlag;
        var rightType = evaluatedRight.TypeFlag;

        if( !AssigningTypeUtility.IsTypeCompatible( leftType, rightType ) )
        {
            CompilerMessageManger.Error(
                expr,
                CompilerMessageResources.semantic_error_assign_type_compatible,
                leftType.ToMessageString(),
                rightType.ToMessageString()
            );

            // 上位のノードで評価を継続させるので代替のノードは生成しない
        }

        return CreateEvaluateNode( evaluatedLeft, evaluatedLeft.TypeFlag );
    }
}
