namespace KSPCompiler.Domain.Ast.Nodes.Expressions
{
    /// <summary>
    /// AST node representing an unary operator: Logical NOT
    /// </summary>
    public class AstUnaryLogicalNotExpressionNode : AstExpressionNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstUnaryLogicalNotExpressionNode( IAstNode parent, AstExpressionNode left, AstExpressionNode right )
            : base( AstNodeId.UnaryLogicalNot, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstUnaryLogicalNotExpressionNode( AstExpressionNode left, AstExpressionNode right )
            : base( AstNodeId.UnaryLogicalNot, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstUnaryLogicalNotExpressionNode()
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
        public override IAstNode Accept( IAstVisitor visitor )
            => visitor.Visit( this );

        #endregion IAstNodeAcceptor
    }
}
