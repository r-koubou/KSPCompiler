namespace KSPCompiler.Domain.Ast.Nodes.Expressions
{
    /// <summary>
    /// AST node representing a division operator (/)
    /// </summary>
    public class AstDivisionExpression : AstExpressionSyntaxNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstDivisionExpression( IAstNode parent, AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.Division, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstDivisionExpression( AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.Division, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstDivisionExpression()
            : base( AstNodeId.Division )
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
