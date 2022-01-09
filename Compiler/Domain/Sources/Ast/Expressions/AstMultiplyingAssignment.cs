namespace KSPCompiler.Domain.Ast.Expressions
{
    /// <summary>
    /// AST node representing an assignment (*=)
    /// </summary>
    public class AstMultiplyingAssignment : AstExpressionSyntaxNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstMultiplyingAssignment( IAstNode parent, AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.MultiplyingAssignment, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstMultiplyingAssignment( AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.MultiplyingAssignment, null, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstMultiplyingAssignment()
            : base( AstNodeId.MultiplyingAssignment, null )
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
