using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Operators;
using KSPCompiler.Domain.Ast.Extensions;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;
using KSPCompiler.Resources;

namespace KSPCompiler.Domain.Ast.Analyzers.Semantics;

public sealed class AssignOperatorEvaluator : IAssignOperatorEvaluator
{
    private IAstVisitor AstVisitor { get; }
    private ICompilerMessageManger CompilerMessageManger { get; }
    private IVariableSymbolTable VariableSymbolTable { get; }

    public AssignOperatorEvaluator(
        IAstVisitor astVisitor,
        ICompilerMessageManger compilerMessageManger,
        IVariableSymbolTable variableSymbolTable )
    {
        AstVisitor            = astVisitor;
        CompilerMessageManger = compilerMessageManger;
        VariableSymbolTable   = variableSymbolTable;
    }

    public IAstNode Evaluate( IAstVisitor<IAstNode> visitor, AstExpressionNode expr, AbortTraverseToken abortTraverseToken )
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

        // 変数の確認
        if( !VariableSymbolTable.TrySearchByName( evaluatedLeft.Name, out var variable ) )
        {
            CompilerMessageManger.Error(
                expr,
                CompilerMessageResources.semantic_error_variable_not_declared,
                evaluatedLeft.Name
            );

            abortTraverseToken.Abort();

            return NullAstExpressionNode.Instance;
        }

        // 定数には代入できない
        if( variable.DataTypeModifier.IsConstant() )
        {
            CompilerMessageManger.Error(
                expr,
                CompilerMessageResources.semantic_error_assign_to_constant,
                expr.Name
            );

            abortTraverseToken.Abort();

            return NullAstExpressionNode.Instance;
        }

        var leftType  = expr.Left.TypeFlag;
        var rightType = evaluatedRight.TypeFlag;

        // コマンドコールの戻り値が複数の型を持つなどで暗黙の型変換を要する場合
        // 代入先の変数に合わせる。暗黙の型変換が不可能な場合は、代替としてVOIDを入れる
        if( ( leftType & rightType ) == 0 )
        {
            rightType = DataTypeFlag.TypeVoid;
        }
        else
        {
            // 暗黙の型変換
            rightType = leftType;
        }

        // 配列要素への格納もあるので配列属性ビットはマスクさせている
        if( leftType.TypeMasked() != rightType.TypeMasked() )
        {
            // 代入先が文字列型なら暗黙の型変換が可能
            // 文字列型以外なら型の不一致 or 条件式は代入不可
            if( !evaluatedLeft.TypeFlag.IsString() || evaluatedRight.TypeFlag.IsBoolean() )
            {
                CompilerMessageManger.Error(
                    expr,
                    CompilerMessageResources.semantic_error_assign_type_compatible,
                    leftType.TypeMasked().ToMessageString(),
                    evaluatedRight.TypeFlag.TypeMasked().ToMessageString()
                );

                abortTraverseToken.Abort();

                return NullAstExpressionNode.Instance;
            }
        }

        variable.State = VariableState.Loaded;

        return evaluatedLeft;
    }
}
