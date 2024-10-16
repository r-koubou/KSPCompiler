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

    public SymbolEvaluator(
        ICompilerMessageManger compilerMessageManger,
        AggregateSymbolTable symbolTable )
    {
        CompilerMessageManger = compilerMessageManger;
        SymbolTable           = symbolTable;
    }

    public IAstNode Evaluate( IAstVisitor<IAstNode> visitor, AstSymbolExpressionNode expr, AbortTraverseToken abortTraverseToken )
    {
        if( TryGetVariableSymbol( visitor, expr, out var result, abortTraverseToken ) )
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

        CompilerMessageManger.Error(
            expr,
            CompilerMessageResources.semantic_error_variable_not_declared,
            expr.Name );

        return NullAstExpressionNode.Instance;
    }

    private bool TryGetVariableSymbol( IAstVisitor<IAstNode> visitor, AstSymbolExpressionNode node, out AstExpressionNode result, AbortTraverseToken abortTraverseToken )
    {
        result = NullAstExpressionNode.Instance;

        if( !SymbolTable.Variables.TrySearchByName( node.Name, out var variable ) )
        {
            return false;
        }

        if( TryGetAstLiteralNode( variable, out result ) )
        {
            return true;
        }

        if( TryGetArrayVariableNode( visitor, node, variable, out result, abortTraverseToken ) )
        {
            return true;
        }

        result          = new AstSymbolExpressionNode();
        result.Name     = variable.Name;
        result.TypeFlag = variable.DataType;

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

    private bool TryGetArrayVariableNode( IAstVisitor<IAstNode> visitor, AstExpressionNode node, VariableSymbol variable, out AstExpressionNode result, AbortTraverseToken abortTraverseToken )
    {
        result = NullAstExpressionNode.Instance;

        if( !variable.DataType.IsArray() )
        {
            return false;
        }

        // 子ノードの評価
        // 含まれる可能性のあるノード
        // - 配列インデックス: AstArrayElementExpressionNode
        if( node.Left.Id != AstNodeId.ArrayElementExpression )
        {
            return false;
        }

        // 配列要素数未確定の状況
        if( variable.ArraySize < 0 )
        {
            CompilerMessageManger.Error(
                node,
                CompilerMessageResources.semantic_error_variable_uninitialized,
                variable.Name
            );
            return false;
        }

        if( node.Left.Accept( visitor, abortTraverseToken ) is not AstExpressionNode indexExpr )
        {
            throw new AstAnalyzeException( node.Left, "Failed to evaluate array index" );
        }

        result          =  new AstSymbolExpressionNode();
        result.Name     =  variable.Name;
        result.TypeFlag =  variable.DataType;
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
                node,
                CompilerMessageResources.semantic_error_variable_array_outofbounds,
                variable.ArraySize,
                intLiteral.Value
            );

            // 変数の型自体は解決しているので return false としない
        }

        return true;
    }

    private bool TryGetPreProcessorSymbol( AstSymbolExpressionNode node, out AstExpressionNode result )
    {
        result = NullAstExpressionNode.Instance;

        if( !SymbolTable.PreProcessorSymbols.TrySearchByName( node.Name, out var symbol ) )
        {
            return false;
        }

        result = new AstSymbolExpressionNode( symbol.Name, node, NullAstExpressionNode.Instance )
        {
            TypeFlag = DataTypeFlag.TypeKspPreprocessorSymbol
        };

        return true;
    }

    private bool TryGetPgsSymbol( AstSymbolExpressionNode node, out AstExpressionNode result )
    {
        result = NullAstExpressionNode.Instance;

        if( !SymbolTable.PgsSymbols.TrySearchByName( node.Name, out var symbol ) )
        {
            return false;
        }

        result = new AstSymbolExpressionNode( symbol.Name, node, NullAstExpressionNode.Instance )
        {
            TypeFlag = DataTypeFlag.TypePgsId
        };

        return true;
    }
}
