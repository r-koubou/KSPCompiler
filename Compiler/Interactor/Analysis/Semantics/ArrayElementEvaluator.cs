using KSPCompiler.Domain.Ast.Extensions;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;
using KSPCompiler.Interactor.Analysis.Commons.Evaluations;
using KSPCompiler.Resources;
using KSPCompiler.UseCases.Analysis.Evaluations.Symbols;

namespace KSPCompiler.Interactor.Analysis.Semantics;

public class ArrayElementEvaluator : IArrayElementEvaluator
{
    private ICompilerMessageManger CompilerMessageManger { get; }
    private IVariableSymbolTable VariableSymbolTable { get; }

    public ArrayElementEvaluator( ICompilerMessageManger compilerMessageManger, IVariableSymbolTable variableSymbolTable )
    {
        CompilerMessageManger = compilerMessageManger;
        VariableSymbolTable   = variableSymbolTable;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstArrayElementExpressionNode expr )
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

        if( expr.Left.Accept( visitor ) is not AstExpressionNode evaluated )
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
