namespace KSPCompiler.Domain.Ast.Node.Expressions
{
    /// <summary>
    /// AST node representing an array element reference expression ( Left: AstSymbolExpression, Right: AstExpressionSyntaxNode )
    /// </summary>
    public class AstArrayElementExpression : AstExpressionSyntaxNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstArrayElementExpression( IAstNode parent, AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.ArrayElementExpression, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstArrayElementExpression( AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.ArrayElementExpression, null, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstArrayElementExpression()
            : base( AstNodeId.ArrayElementExpression, null )
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
