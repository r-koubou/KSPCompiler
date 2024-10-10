namespace KSPCompiler.Domain.Ast.Nodes.Expressions
{
    /// <summary>
    /// AST node representing a comparison operator: greater than (&gt;)
    /// </summary>
    public class AstGreaterThanExpressionNode : AstExpressionNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstGreaterThanExpressionNode( IAstNode parent, AstExpressionNode left, AstExpressionNode right )
            : base( AstNodeId.GreaterThan, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstGreaterThanExpressionNode( AstExpressionNode left, AstExpressionNode right )
            : base( AstNodeId.GreaterThan, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstGreaterThanExpressionNode()
            : base( AstNodeId.GreaterThan )
        {
        }

        #region IAstNodeAcceptor

        ///
        /// <inheritdoc />
        ///
        public override int ChildNodeCount
            => 2;

        ///
        /// <inheritdoc/>
        ///
        public override T Accept<T>( IAstVisitor<T> visitor, AbortTraverseToken abortTraverseToken )
            => visitor.Visit( this , abortTraverseToken );

        #endregion IAstNodeAcceptor
    }
}