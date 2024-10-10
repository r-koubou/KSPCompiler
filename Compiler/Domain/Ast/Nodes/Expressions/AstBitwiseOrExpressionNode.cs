namespace KSPCompiler.Domain.Ast.Nodes.Expressions
{
    /// <summary>
    /// AST node representing a bitwise OR operator
    /// </summary>
    public class AstBitwiseOrExpressionNode : AstExpressionNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstBitwiseOrExpressionNode( IAstNode parent, AstExpressionNode left, AstExpressionNode right )
            : base( AstNodeId.BitwiseOr, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstBitwiseOrExpressionNode( AstExpressionNode left, AstExpressionNode right )
            : base( AstNodeId.BitwiseOr, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstBitwiseOrExpressionNode()
            : base( AstNodeId.BitwiseOr )
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
