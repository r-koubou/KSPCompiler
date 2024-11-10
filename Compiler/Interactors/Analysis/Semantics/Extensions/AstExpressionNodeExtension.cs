using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Interactors.Analysis.Commons.Extensions;
using KSPCompiler.Resources;

namespace KSPCompiler.Interactors.Analysis.Semantics.Extensions;

public static class AstExpressionNodeExtension
{
    public static bool EvaluateSymbolStateIsInitialized( this AstExpressionNode self, IAstNode parent, ICompilerMessageManger compilerMessageManger )
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
