namespace KSPCompiler.Domain.Ast.Nodes.Expressions
{
    /// <summary>
    /// AST node representing an unary operator: NOT
    /// </summary>
    public class AstUnaryNotExpressionNode : AstExpressionNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstUnaryNotExpressionNode( IAstNode parent, AstExpressionNode left, AstExpressionNode right )
            : base( AstNodeId.UnaryNot, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstUnaryNotExpressionNode( AstExpressionNode left, AstExpressionNode right )
            : base( AstNodeId.UnaryNot, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstUnaryNotExpressionNode()
            : base( AstNodeId.UnaryNot )
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