using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;

namespace KSPCompiler.Interactors.Analysis.Semantics.Extensions;

public static class AstCompilationUnitNodeExtension
{
    /// <summary>
    /// Traverse the children of the node for semantic analysis, preferring the Init callback
    /// </summary>
    /// <remarks>
    /// Default accept children method will traverse all children in the order they were found.
    /// This method will first traverse the Init callback if it exists, then traverse the rest of the children.
    /// </remarks>
    /// <seealso cref="AstCompilationUnitNode.AcceptChildren"/>
    public static void AcceptChildrenPreferInitCallback( this AstCompilationUnitNode self, IAstVisitor visitor )
    {
        AstCallbackDeclarationNode? initCallBackDeclare = null;

        // init コールバックの宣言を見つける
        foreach( var block in self.GlobalBlocks )
        {
            if( block is not AstCallbackDeclarationNode callback )
            {
                continue;
            }

            if( callback.Name == "Init" )
            {
                initCallBackDeclare = callback;
            }
        }

        // init コールバックの宣言を先に処理（見つかれば）
        // 変数の宣言は init コールバックのでのみ可能なため
        initCallBackDeclare?.Accept( visitor );

        // それ以外のグローバルブロックを処理
        foreach( var block in self.GlobalBlocks )
        {
            if( block != initCallBackDeclare )
            {
                block.Accept( visitor );
            }
        }
    }
}
