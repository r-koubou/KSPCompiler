namespace KSPCompiler.Domain.Ast.Expressions
{
    /// <summary>
    /// AST node representing a comparison operator: greater equal (&gt;=)
    /// </summary>
    public class AstGreaterEqualExpression : AstExpressionSyntaxNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstGreaterEqualExpression( IAstNode parent, AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.GreaterEqual, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstGreaterEqualExpression( AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.GreaterEqual, null, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstGreaterEqualExpression()
            : base( AstNodeId.GreaterEqual, null )
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
