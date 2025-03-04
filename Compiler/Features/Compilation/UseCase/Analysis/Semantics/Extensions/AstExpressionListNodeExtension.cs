using System;
using System.Collections.Generic;

using KSPCompiler.Shared.Domain.Ast.Nodes;
using KSPCompiler.Shared.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Semantics.Extensions;

/// <summary>
/// If evaluated arguments are literals, replace the convoluted expressions with the evaluated nodes.
/// </summary>
public static class AstExpressionListNodeExtension
{
    public static void ReplaceConvolutedExpressions(
        this AstExpressionListNode self,
        IReadOnlyList<AstExpressionNode> sourceEvaluatedNodes,
        AstExpressionListNode destExpressionListNode )
    {
        if( destExpressionListNode.Expressions.Count != sourceEvaluatedNodes.Count )
        {
            throw new InvalidOperationException( "The number of expressions does not match the number of evaluated nodes." );
        }

        for( var i = 0; i < sourceEvaluatedNodes.Count; i++ )
        {
            if( sourceEvaluatedNodes[ i ].IsLiteralNode() )
            {
                destExpressionListNode.Expressions.Put( i, sourceEvaluatedNodes[ i ] );
            }
        }
    }
}
