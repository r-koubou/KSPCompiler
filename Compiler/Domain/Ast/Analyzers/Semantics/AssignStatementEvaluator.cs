using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Assigns;
using KSPCompiler.Domain.Ast.Extensions;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;
using KSPCompiler.Resources;

namespace KSPCompiler.Domain.Ast.Analyzers.Semantics;

public sealed class AssignStatementEvaluator : IAssignStatementEvaluator
{
    private ICompilerMessageManger CompilerMessageManger { get; }

    private static AstExpressionNode CreateEvaluateNode( AstExpressionNode source, DataTypeFlag type )
    {
        var result = source.Clone<AstExpressionNode>();
        result.TypeFlag = type;

        return result;
    }

    public AssignStatementEvaluator( ICompilerMessageManger compilerMessageManger )
    {
        CompilerMessageManger = compilerMessageManger;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstAssignStatementNode statement )
    {
        /*
                       :=
                       +
                       |
                  +----+----+
                  |         |
              expr.Left   expr.Right
              (variable)    (value)
        */

        if( statement.Left.Accept( visitor ) is not AstExpressionNode evaluatedLeft )
        {
            throw new AstAnalyzeException( statement, "Failed to evaluate left side of binary operator" );
        }

        if( statement.Right.Accept( visitor ) is not AstExpressionNode evaluatedRight )
        {
            throw new AstAnalyzeException( statement, "Failed to evaluate right side of binary operator" );
        }

        // 定数には代入できない
        if( evaluatedLeft.Constant )
        {
            CompilerMessageManger.Error(
                statement,
                CompilerMessageResources.semantic_error_assign_to_constant,
                evaluatedLeft.Name
            );

            return CreateEvaluateNode( evaluatedLeft, evaluatedLeft.TypeFlag );
        }

        var leftType  = evaluatedLeft.TypeFlag;
        var rightType = evaluatedRight.TypeFlag;

        if( !TypeCompatibility.IsAssigningTypeCompatible( leftType, rightType ) )
        {
            CompilerMessageManger.Error(
                statement,
                CompilerMessageResources.semantic_error_assign_type_compatible,
                leftType.ToMessageString(),
                rightType.ToMessageString()
            );

            // 上位のノードで評価を継続させるので代替のノードは生成しない
        }

        return CreateEvaluateNode( evaluatedLeft, evaluatedLeft.TypeFlag );
    }
}
