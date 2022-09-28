namespace KSPCompiler.Domain.Ast.Node.Expressions
{
    /// <summary>
    /// AST node representing a subtraction operator (-)
    /// </summary>
    public class AstSubtractionExpression : AstExpressionSyntaxNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstSubtractionExpression( IAstNode parent, AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.Subtraction, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstSubtractionExpression( AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.Subtraction, null, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstSubtractionExpression()
            : base( AstNodeId.Subtraction, null )
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
