namespace KSPCompiler.Domain.Ast.Nodes.Expressions
{
    /// <summary>
    /// AST node representing an unary operator: Logical NOT
    /// </summary>
    public class AstUnaryLogicalNotExpression : AstExpressionSyntaxNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstUnaryLogicalNotExpression( IAstNode parent, AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.UnaryLogicalNot, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstUnaryLogicalNotExpression( AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.UnaryLogicalNot, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstUnaryLogicalNotExpression()
            : base( AstNodeId.UnaryLogicalNot )
        {
        }

        #region IAstNodeAcceptor
        ///
        /// <inheritdoc />
        ///
        public override int ChildNodeCount
            => 1;

        ///
        /// <inheritdoc/>
        ///
        public override T Accept<T>( IAstVisitor<T> visitor, AbortTraverseToken abortTraverseToken )
            => visitor.Visit( this , abortTraverseToken );

        #endregion IAstNodeAcceptor
    }
}
