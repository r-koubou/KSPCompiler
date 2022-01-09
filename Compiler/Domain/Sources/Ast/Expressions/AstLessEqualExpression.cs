namespace KSPCompiler.Domain.Ast.Expressions
{
    /// <summary>
    /// AST node representing a comparison operator: less equal (&lt;=)
    /// </summary>
    public class AstLessEqualExpression : AstExpressionSyntaxNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstLessEqualExpression( IAstNode parent, AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.LessEqual, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstLessEqualExpression( AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.LessEqual, null, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstLessEqualExpression()
            : base( AstNodeId.LessEqual, null )
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
