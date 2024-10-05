namespace KSPCompiler.Domain.Ast.Nodes.Expressions
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
            : base( AstNodeId.Subtraction, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstSubtractionExpression()
            : base( AstNodeId.Subtraction )
        {
        }

        #region IAstNodeAcceptor

        ///
        /// <inheritdoc/>
        ///
        public override T Accept<T>( IAstVisitor<T> visitor, AbortTraverseToken abortTraverseToken )
            => visitor.Visit( this , abortTraverseToken );

        #endregion IAstNodeAcceptor
    }
}
