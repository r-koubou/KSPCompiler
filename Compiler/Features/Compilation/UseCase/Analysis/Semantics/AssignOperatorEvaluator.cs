using KSPCompiler.Features.Compilation.Domain.Ast.Nodes;
using KSPCompiler.Features.Compilation.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Features.Compilation.Domain.Symbols;
using KSPCompiler.Features.Compilation.Domain.Symbols.MetaData;
using KSPCompiler.Features.Compilation.Domain.Symbols.MetaData.Extensions;
using KSPCompiler.Features.Compilation.UseCase.Abstractions.Evaluations.Operators;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Commons.Evaluations;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Extensions;
using KSPCompiler.Shared.EventEmitting;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Semantics;

public sealed class AssignOperatorEvaluator : IAssignOperatorEvaluator
{
    private IEventEmitter EventEmitter { get; }

    private AggregateSymbolTable SymbolTable { get; }

    private static AstExpressionNode CreateEvaluateNode( AstExpressionNode source, DataTypeFlag type )
    {
        var result = source.Clone<AstExpressionNode>();
        result.TypeFlag = type;

        return result;
    }

    public AssignOperatorEvaluator( IEventEmitter eventEmitter, AggregateSymbolTable symbolTable )
    {
        EventEmitter = eventEmitter;
        SymbolTable  = symbolTable;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstExpressionNode expr )
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
            EventEmitter.Emit(
                expr.AsErrorEvent(
                    CompilerMessageResources.semantic_error_assign_to_constant,
                    evaluatedLeft.Name
                )
            );

            return CreateEvaluateNode( evaluatedLeft, evaluatedLeft.TypeFlag );
        }

        // ビルトイン変数には代入できない
        if( evaluatedLeft is AstSymbolExpressionNode { BuiltIn: true } symbolLeft )
        {
            EventEmitter.Emit(
                expr.AsErrorEvent(
                    CompilerMessageResources.semantic_error_assign_to_builtin_variable,
                    symbolLeft.Name
                )
            );

            return CreateEvaluateNode( evaluatedLeft, evaluatedLeft.TypeFlag );
        }

        var leftType  = evaluatedLeft.TypeFlag;
        var rightType = evaluatedRight.TypeFlag;

        if( !TypeCompatibility.IsAssigningTypeCompatible( leftType, rightType ) )
        {
            EventEmitter.Emit(
                expr.AsErrorEvent(
                    CompilerMessageResources.semantic_error_assign_type_compatible,
                    leftType.ToMessageString(),
                    rightType.ToMessageString()
                )
            );

            // 上位のノードで評価を継続させるので代替のノードは生成しない
        }

        // 代入先変数の状態を更新
        if( evaluatedLeft is AstSymbolExpressionNode symbolNode )
        {
            if( SymbolTable.TrySearchVariableByName( symbolNode.Name, out var variable ) )
            {
                variable.State = SymbolState.Loaded;
            }
        }

        // 代入値が畳み込みされた値（リテラル値）であれば、右項をその値に置き換える
        if( evaluatedRight.IsLiteralNode() )
        {
            expr.Right = evaluatedRight;
        }

        return CreateEvaluateNode( evaluatedLeft, evaluatedLeft.TypeFlag );
    }
}
