namespace KSPCompiler.Domain.Ast.Node.Expressions
{
    /// <summary>
    /// AST node representing an invoking the KSP command
    /// </summary>
    public class AstCallExpression : AstExpressionSyntaxNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstCallExpression( IAstNode parent, AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.CallExpression, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstCallExpression( AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.CallExpression, null, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstCallExpression()
            : base( AstNodeId.CallExpression, null )
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
