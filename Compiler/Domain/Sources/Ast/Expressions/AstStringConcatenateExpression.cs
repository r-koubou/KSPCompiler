namespace KSPCompiler.Domain.Ast.Expressions
{
    /// <summary>
    /// AST node representing a string concatenation operator (&)
    /// </summary>
    public class AstStringConcatenateExpression : AstExpressionSyntaxNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstStringConcatenateExpression( IAstNode parent, AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.StringConcatenate, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstStringConcatenateExpression( AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.StringConcatenate, null, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstStringConcatenateExpression()
            : base( AstNodeId.StringConcatenate, null )
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
