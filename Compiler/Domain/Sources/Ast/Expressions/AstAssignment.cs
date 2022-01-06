#nullable disable

namespace KSPCompiler.Domain.Ast.Expressions
{
    /// <summary>
    /// AST node representing an assignment (:=)
    /// </summary>
    public class AstAssignment : AstExpressionSyntaxNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstAssignment( IAstNode parent, AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.Assignment, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstAssignment( AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.Assignment, null, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstAssignment()
            : base( AstNodeId.Assignment, null, null, null )
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
