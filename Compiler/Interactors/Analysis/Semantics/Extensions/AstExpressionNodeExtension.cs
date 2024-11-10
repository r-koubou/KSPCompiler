using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Interactors.Analysis.Commons.Extensions;
using KSPCompiler.Resources;

namespace KSPCompiler.Interactors.Analysis.Semantics.Extensions;

public static class AstExpressionNodeExtension
{
    /// <summary>
    /// Evaluate the symbol state is initialized or not if the expression is a <see cref="AstSymbolExpressionNode"/>.
    /// </summary>
    /// <returns>true if the symbol state is initialized or node is not a <see cref="AstSymbolExpressionNode"/>. Otherwise, false.</returns>
    public static bool EvaluateSymbolState( this AstExpressionNode self, IAstNode parent, ICompilerMessageManger compilerMessageManger )
    {
        if( self is not AstSymbolExpressionNode symbolNode )
        {
            return true;
        }

        if( symbolNode.SymbolState == SymbolState.UnInitialized )
        {
            compilerMessageManger.Error(
                parent,
                CompilerMessageResources.semantic_error_variable_uninitialized,
                symbolNode.Name
            );

            return false;
        }

        return true;
    }
}
