namespace KSPCompiler.Domain.Ast.Expressions
{
    /// <summary>
    /// AST node representing an unary operator: negative
    /// </summary>
    public class AstUnaryMinusExpression : AstExpressionSyntaxNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstUnaryMinusExpression( IAstNode parent, AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.UnaryMinus, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstUnaryMinusExpression( AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.UnaryMinus, null, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstUnaryMinusExpression()
            : base( AstNodeId.UnaryMinus, null )
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
