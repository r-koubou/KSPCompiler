namespace KSPCompiler.Shared.Domain.Ast.Nodes.Expressions
{
    /// <summary>
    /// AST node representing an addition operator (+)
    /// </summary>
    public class AstAdditionExpressionNode : AstExpressionNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstAdditionExpressionNode( IAstNode parent, AstExpressionNode left, AstExpressionNode right )
            : base( AstNodeId.Addition, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstAdditionExpressionNode( AstExpressionNode left, AstExpressionNode right )
            : base( AstNodeId.Addition, NullAstNode.Instance, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstAdditionExpressionNode()
            : base( AstNodeId.Addition )
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
