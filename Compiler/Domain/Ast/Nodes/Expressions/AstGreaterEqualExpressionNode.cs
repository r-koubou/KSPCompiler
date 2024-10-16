namespace KSPCompiler.Domain.Ast.Nodes.Expressions
{
    /// <summary>
    /// AST node representing a comparison operator: greater equal (&gt;=)
    /// </summary>
    public class AstGreaterEqualExpressionNode : AstExpressionNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstGreaterEqualExpressionNode( IAstNode parent, AstExpressionNode left, AstExpressionNode right )
            : base( AstNodeId.GreaterEqual, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstGreaterEqualExpressionNode( AstExpressionNode left, AstExpressionNode right )
            : base( AstNodeId.GreaterEqual, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstGreaterEqualExpressionNode()
            : base( AstNodeId.GreaterEqual )
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
        public override T Accept<T>( IAstVisitor<T> visitor )
            => visitor.Visit( this );

        #endregion IAstNodeAcceptor
    }
}
