namespace KSPCompiler.Domain.Ast.Nodes.Expressions
{
    /// <summary>
    /// AST node representing a comparison operator: equal
    /// </summary>
    public class AstEqualExpressionNode : AstExpressionNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstEqualExpressionNode( IAstNode parent, AstExpressionNode left, AstExpressionNode right )
            : base( AstNodeId.Equal, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstEqualExpressionNode( AstExpressionNode left, AstExpressionNode right )
            : base( AstNodeId.Equal, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstEqualExpressionNode()
            : base( AstNodeId.Equal )
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
        public override IAstNode Accept( IAstVisitor visitor )
            => visitor.Visit( this );

        #endregion IAstNodeAcceptor
    }
}
