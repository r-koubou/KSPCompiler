using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Symbols;
using KSPCompiler.Domain.Ast.Extensions;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;
using KSPCompiler.Resources;

namespace KSPCompiler.Domain.Ast.Analyzers.Semantics;

public class ArrayElementEvaluator : IArrayElementEvaluator
{
    private ICompilerMessageManger CompilerMessageManger { get; }
    private IVariableSymbolTable VariableSymbolTable { get; }

    public ArrayElementEvaluator( ICompilerMessageManger compilerMessageManger, IVariableSymbolTable variableSymbolTable )
    {
        CompilerMessageManger = compilerMessageManger;
        VariableSymbolTable   = variableSymbolTable;
    }

    public IAstNode Evaluate( IAstVisitor<IAstNode> visitor, AstArrayElementExpressionNode expr, AbortTraverseToken abortTraverseToken )
    {
        /*
               symbol
                 +
                 |
                 |
                [ ] <-- here
                 +
                 |
                 |
              expr.Left

        */

        if( expr.Parent is not AstSymbolExpressionNode symbolNode )
        {
            throw new AstAnalyzeException( expr, "Array element must be a child of a symbol" );
        }

        if( expr.Left.Accept( visitor, abortTraverseToken ) is not AstExpressionNode evaluated )
        {
            throw new AstAnalyzeException( expr, "Failed to evaluate left side of array element" );
        }

        if( evaluated is AstStringLiteralNode or AstRealLiteralNode )
        {
            CompilerMessageManger.Error(
                expr,
                CompilerMessageResources.semantic_error_binaryoprator_compatible,
                evaluated.TypeFlag.ToMessageString()
            );

            abortTraverseToken.Abort();
            return NullAstExpressionNode.Instance;
        }

        if( evaluated is not AstIntLiteralNode intLiteralNode )
        {
            return evaluated;
        }

        if( !VariableSymbolTable.TrySearchByName( symbolNode.Name, out var variable ) )
        {
            CompilerMessageManger.Error(
                symbolNode,
                CompilerMessageResources.semantic_error_variable_not_declared,
                symbolNode.Name
            );

            abortTraverseToken.Abort();
            return NullAstExpressionNode.Instance;
        }

        if( intLiteralNode.Value < 0 || intLiteralNode.Value >= variable.ArraySize )
        {
            CompilerMessageManger.Error(
                expr,
                CompilerMessageResources.semantic_error_variable_arrayoutofbounds,
                variable.ArraySize,
                intLiteralNode.Value
            );

            abortTraverseToken.Abort();
            return NullAstExpressionNode.Instance;
        }

        return intLiteralNode;
    }
}
