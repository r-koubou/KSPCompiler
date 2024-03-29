namespace KSPCompiler.Domain.Ast.Node.Expressions
{
    /// <summary>
    /// AST node representing a bitwise OR operator
    /// </summary>
    public class AstBitwiseOrExpression : AstExpressionSyntaxNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstBitwiseOrExpression( IAstNode parent, AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.BitwiseOr, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstBitwiseOrExpression( AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.BitwiseOr, null, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstBitwiseOrExpression()
            : base( AstNodeId.BitwiseOr, null )
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
