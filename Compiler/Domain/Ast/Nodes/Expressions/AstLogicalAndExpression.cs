namespace KSPCompiler.Domain.Ast.Nodes.Expressions
{
    /// <summary>
    /// AST node representing a conditional operator: AND (&&)
    /// </summary>
    public class AstLogicalAndExpression : AstExpressionSyntaxNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstLogicalAndExpression( IAstNode parent, AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.LogicalAnd, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstLogicalAndExpression( AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.LogicalAnd, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstLogicalAndExpression()
            : base( AstNodeId.LogicalAnd )
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
