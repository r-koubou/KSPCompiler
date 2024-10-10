namespace KSPCompiler.Domain.Ast.Nodes.Expressions
{
    /// <summary>
    /// AST node representing a comparison operator: not equal
    /// </summary>
    public class AstNotEqualExpressionNode : AstExpressionNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstNotEqualExpressionNode( IAstNode parent, AstExpressionNode left, AstExpressionNode right )
            : base( AstNodeId.NotEqual, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstNotEqualExpressionNode( AstExpressionNode left, AstExpressionNode right )
            : base( AstNodeId.NotEqual, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstNotEqualExpressionNode()
            : base( AstNodeId.NotEqual )
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
