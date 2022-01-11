namespace KSPCompiler.Domain.Ast.Node.Expressions
{
    /// <summary>
    /// AST node representing an addition operator (+)
    /// </summary>
    public class AstAdditionExpression : AstExpressionSyntaxNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstAdditionExpression( IAstNode parent, AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.Addition, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstAdditionExpression( AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.Addition, null, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstAdditionExpression()
            : base( AstNodeId.Addition, null )
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
