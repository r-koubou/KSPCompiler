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

        // 子ノードの評価
        // 含まれる可能性のあるノード
        // - 配列インデックス: AstArrayElementExpressionNode
        node.AcceptChildren( visitor, abortTraverseToken );

        result          = new AstSymbolExpressionNode();
        result.Name     = variable.Name;
        result.TypeFlag = variable.DataType;

        // 配列インデックスを式に含んでいる場合、要素アクセスになるので評価結果から配列フラグを削除
        if( node.Left.Id == AstNodeId.ArrayElementExpression )
        {
            result.TypeFlag &= ~DataTypeFlag.AttributeArray;
        }

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

    private bool TryGetPreProcessorSymbol( AstSymbolExpressionNode node, out AstExpressionNode result )
    {
        result = NullAstExpressionNode.Instance;
        return false;
    }

    private bool TryGetPgsSymbol( AstSymbolExpressionNode node, out AstExpressionNode result )
    {
        result = NullAstExpressionNode.Instance;
        return false;
    }
}
