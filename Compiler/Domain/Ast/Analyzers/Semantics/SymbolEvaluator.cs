using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Symbols;
using KSPCompiler.Domain.Ast.Extensions;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;
using KSPCompiler.Resources;

namespace KSPCompiler.Domain.Ast.Analyzers.Semantics;

public class SymbolEvaluator : ISymbolEvaluator
{
    private ICompilerMessageManger CompilerMessageManger { get; }
    private AggregateSymbolTable SymbolTable { get; }

    private static AstExpressionNode CreateEvaluateNode( AstExpressionNode source, SymbolBase symbol )
    {
        var result = new AstSymbolExpressionNode( symbol.Name, source.Left )
        {
            Parent   = source.Parent,
            TypeFlag = symbol.DataType
        };

        return result;
    }

    private static AstExpressionNode CreateEvaluateNode( AstExpressionNode source, DataTypeFlag type )
        => new AstSymbolExpressionNode( source.Name, source.Left )
        {
            Parent   = source.Parent,
            TypeFlag = type
        };

    public SymbolEvaluator(
        ICompilerMessageManger compilerMessageManger,
        AggregateSymbolTable symbolTable )
    {
        CompilerMessageManger = compilerMessageManger;
        SymbolTable           = symbolTable;
    }

    public IAstNode Evaluate( IAstVisitor<IAstNode> visitor, AstSymbolExpressionNode expr )
    {
        if( TryGetVariableSymbol( visitor, expr, out var result ) )
        {
            return result;
        }

        if( TryGetPreProcessorSymbol( expr, out result ) )
        {
            return result;
        }

        if( TryGetPgsSymbol( expr, out result ) )
        {
            return result;
        }

        // シンボルが見つからなかったのでエラー計上後、代替の評価結果を返す

        CompilerMessageManger.Error(
            expr,
            CompilerMessageResources.semantic_error_variable_not_declared,
            expr.Name );

        result = CreateEvaluateNode( expr, DataTypeUtility.GuessFromSymbolName( expr.Name ) );

        return result;
    }

    private bool TryGetVariableSymbol( IAstVisitor<IAstNode> visitor, AstSymbolExpressionNode expr, out AstExpressionNode result )
    {
        result = NullAstExpressionNode.Instance;

        if( !SymbolTable.Variables.TrySearchByName( expr.Name, out var variable ) )
        {
            return false;
        }

        // 変数は見つかったが、未初期化の場合はエラー
        if( variable.State == VariableState.UnInitialized )
        {
            CompilerMessageManger.Error(
                expr,
                CompilerMessageResources.semantic_error_variable_uninitialized,
                variable.Name
            );

            // 変数は見つかったが、エラー扱いとして代替の評価結果を返す
            result = CreateEvaluateNode( expr, variable );
            return true;
        }

        variable.State = VariableState.Loaded;
        if( TryGetAstLiteralNode( variable, out result ) )
        {
            return true;
        }

        if( TryGetArrayVariableNode( visitor, expr, variable, out result ) )
        {
            return true;
        }

        result = CreateEvaluateNode( expr, variable );

        return true;
    }

    private bool TryGetAstLiteralNode( VariableSymbol variable, out AstExpressionNode result )
    {
        result = NullAstExpressionNode.Instance;

        if( variable.Reserved || !variable.DataTypeModifier.IsConstant() )
        {
            return false;
        }

        switch( variable.Value )
        {
            case int intValue:
                result = new AstIntLiteralNode( intValue );
                return true;
            case double doubleValue:
                result = new AstRealLiteralNode( doubleValue );
                return true;
            case string stringValue:
                result = new AstStringLiteralNode( stringValue );
                return true;
            default:
                return false;
        }
    }

    private bool TryGetArrayVariableNode( IAstVisitor<IAstNode> visitor, AstExpressionNode expr, VariableSymbol variable, out AstExpressionNode result )
    {
        result = NullAstExpressionNode.Instance;

        if( !variable.DataType.IsArray() )
        {
            return false;
        }

        // 子ノードの評価
        // 含まれる可能性のあるノード
        // - 配列インデックス: AstArrayElementExpressionNode

        // 添字の式を持っていない場合は配列型として返す
        if( expr.Left.Id != AstNodeId.ArrayElementExpression )
        {
            result = CreateEvaluateNode( expr, variable );

            return true;
        }

        // 配列要素数未確定の状況
        if( variable.State == VariableState.UnInitialized )
        {
            CompilerMessageManger.Error(
                expr,
                CompilerMessageResources.semantic_error_variable_uninitialized,
                variable.Name
            );
            return false;
        }

        if( expr.Left.Accept( visitor ) is not AstExpressionNode indexExpr )
        {
            throw new AstAnalyzeException( expr.Left, "Failed to evaluate array index" );
        }

        result = CreateEvaluateNode( expr, variable );
        // 配列インデックスを式に含んでいる場合、要素アクセスになるので評価結果から配列フラグを削除
        result.TypeFlag &= ~DataTypeFlag.AttributeArray;

        // 変数がビルトイン変数または要素アクセスがリテラルで確定していない場合は評価はここまで
        if( variable.Reserved || indexExpr is not AstIntLiteralNode intLiteral )
        {
            return true;
        }

        // 配列要素の範囲チェック
        if( intLiteral.Value < 0 || intLiteral.Value >= variable.ArraySize )
        {
            CompilerMessageManger.Error(
                expr,
                CompilerMessageResources.semantic_error_variable_array_outofbounds,
                variable.ArraySize,
                intLiteral.Value
            );

            // 変数の型自体は解決しているので return false としない
        }

        return true;
    }

    private bool TryGetPreProcessorSymbol( AstSymbolExpressionNode expr, out AstExpressionNode result )
    {
        result = NullAstExpressionNode.Instance;

        if( !SymbolTable.PreProcessorSymbols.TrySearchByName( expr.Name, out var symbol ) )
        {
            return false;
        }

        result = CreateEvaluateNode( expr, symbol );

        return true;
    }

    private bool TryGetPgsSymbol( AstSymbolExpressionNode expr, out AstExpressionNode result )
    {
        result = NullAstExpressionNode.Instance;

        if( !SymbolTable.PgsSymbols.TrySearchByName( expr.Name, out var symbol ) )
        {
            return false;
        }

        result = CreateEvaluateNode( expr, symbol );

        return true;
    }
}
