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

        if( expr.Left.Accept( visitor, abortTraverseToken ) is not AstExpressionNode evaluated )
        {
            throw new AstAnalyzeException( expr, "Failed to evaluate left side of array element" );
        }

        if( !evaluated.TypeFlag.IsInt() )
        {
            CompilerMessageManger.Error(
                expr,
                CompilerMessageResources.semantic_error_array_subscript_compatible,
                evaluated.TypeFlag.ToMessageString()
            );

            return NullAstExpressionNode.Instance;
        }

        return evaluated;
    }
}
