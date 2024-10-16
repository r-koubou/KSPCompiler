namespace KSPCompiler.Domain.Ast.Nodes.Expressions
{
    /// <summary>
    /// AST node representing a bitwise XOR operator (.xor.)
    /// </summary>
    public class AstBitwiseXorExpressionNode : AstExpressionNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstBitwiseXorExpressionNode( IAstNode parent, AstExpressionNode left, AstExpressionNode right )
            : base( AstNodeId.BitwiseXor, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstBitwiseXorExpressionNode( AstExpressionNode left, AstExpressionNode right )
            : base( AstNodeId.BitwiseXor, NullAstNode.Instance, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstBitwiseXorExpressionNode()
            : base( AstNodeId.BitwiseXor, NullAstNode.Instance )
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
