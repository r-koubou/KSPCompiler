namespace KSPCompiler.Domain.Ast.Node.Expressions
{
    /// <summary>
    /// AST node representing a comparison operator: less than (&lt;)
    /// </summary>
    public class AstLessThanExpression : AstExpressionSyntaxNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstLessThanExpression( IAstNode parent, AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.LessThan, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstLessThanExpression( AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.LessThan, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstLessThanExpression()
            : base( AstNodeId.LessThan )
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
