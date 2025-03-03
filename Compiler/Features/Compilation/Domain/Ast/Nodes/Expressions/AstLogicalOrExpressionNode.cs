namespace KSPCompiler.Features.Compilation.Domain.Ast.Nodes.Expressions
{
    /// <summary>
    /// AST node representing a conditional operator: AND (||)
    /// </summary>
    public class AstLogicalOrExpressionNode : AstExpressionNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstLogicalOrExpressionNode( IAstNode parent, AstExpressionNode left, AstExpressionNode right )
            : base( AstNodeId.LogicalOr, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstLogicalOrExpressionNode( AstExpressionNode left, AstExpressionNode right )
            : base( AstNodeId.LogicalOr, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstLogicalOrExpressionNode()
            : base( AstNodeId.LogicalOr )
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
