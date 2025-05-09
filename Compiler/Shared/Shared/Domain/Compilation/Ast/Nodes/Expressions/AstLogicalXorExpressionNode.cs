namespace KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions
{
    /// <summary>
    /// AST node representing a conditional operator: XOR (xor)
    /// </summary>
    public class AstLogicalXorExpressionNode : AstExpressionNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstLogicalXorExpressionNode( IAstNode parent, AstExpressionNode left, AstExpressionNode right )
            : base( AstNodeId.LogicalXor, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstLogicalXorExpressionNode( AstExpressionNode left, AstExpressionNode right )
            : base( AstNodeId.LogicalXor, NullAstNode.Instance, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstLogicalXorExpressionNode()
            : base( AstNodeId.LogicalXor, NullAstNode.Instance )
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
