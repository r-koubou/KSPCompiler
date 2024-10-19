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

        // 暗黙の型変換
        // コマンドコールの戻り値が複数の型を持つなどで暗黙の型変換を要する場合
        // 代入先の変数に合わせる。暗黙の型変換が不可能な場合は、以降の型判定で失敗させる
        if( ( leftType & rightType ) != 0 )
        {
            // 型情報のマージ
            rightType |= leftType.TypeMasked(); // Mask: 型以外の属性は含まない
        }

        // 完全一致
        if( leftType == rightType )
        {
            return CreateEvaluateNode( evaluatedLeft, evaluatedLeft.TypeFlag );
        }

        // 個別に型チェック
        if( !IsTypeMatched( leftType, rightType ) )
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

    private static bool IsTypeMatched( DataTypeFlag leftType, DataTypeFlag rightType )
    {
        var result = true;

        // 代入先：配列型
        // 代入元：非配列型
        if( leftType.IsArray() && !rightType.IsArray() )
        {
            result = false;
        }
        // 代入先：非配列型
        // 代入元：配列型
        else if( rightType.IsArray() && !leftType.IsArray() )
        {
            result = false;
        }
        // 型が一致するビットがない
        else if( ( leftType.TypeMasked() & rightType.TypeMasked() ) == 0 )
        {
            // 文字列型へ integer, real 値の代入は可能（文字列への型変換が発生）
            // -> 代入先が文字列型以外なら型の不一致 or 条件式は代入不可
            if( !leftType.IsString() || rightType.IsBoolean() )
            {
                result = false;
            }
        }

        return result;
    }
}
