using System;

namespace KSPCompiler.Domain.Ast.Node.Expressions
{
    /// <summary>
    /// AST node representing an assignment (-=)
    /// </summary>
    public class AstSubtractionAssignment : AstExpressionSyntaxNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstSubtractionAssignment( IAstNode parent, AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.SubtractionAssignment, parent, left, right )
        {
            throw new NotImplementedException( "`-=` is not KSP Syntax (Tt will be extended syntax in the future)" );
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstSubtractionAssignment( AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.SubtractionAssignment, null, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstSubtractionAssignment()
            : base( AstNodeId.SubtractionAssignment, null )
        {
        }

        #region IAstNodeAcceptor

        ///
        /// <inheritdoc/>
        ///
        public override T Accept<T>( IAstVisitor<T> visitor )
            => visitor.Visit( this );

        #endregion IAstNodeAcceptor
    }
}
