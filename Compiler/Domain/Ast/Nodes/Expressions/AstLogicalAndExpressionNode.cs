namespace KSPCompiler.Domain.Ast.Nodes.Expressions
{
    /// <summary>
    /// AST node representing a conditional operator: AND (&&)
    /// </summary>
    public class AstLogicalAndExpressionNode : AstExpressionNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstLogicalAndExpressionNode( IAstNode parent, AstExpressionNode left, AstExpressionNode right )
            : base( AstNodeId.LogicalAnd, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstLogicalAndExpressionNode( AstExpressionNode left, AstExpressionNode right )
            : base( AstNodeId.LogicalAnd, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstLogicalAndExpressionNode()
            : base( AstNodeId.LogicalAnd )
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