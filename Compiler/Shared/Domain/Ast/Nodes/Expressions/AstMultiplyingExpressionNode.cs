namespace KSPCompiler.Features.Compilation.Domain.Ast.Nodes.Expressions
{
    /// <summary>
    /// AST node representing a multiplying operator (*)
    /// </summary>
    public class AstMultiplyingExpressionNode : AstExpressionNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstMultiplyingExpressionNode( IAstNode parent, AstExpressionNode left, AstExpressionNode right )
            : base( AstNodeId.Multiplying, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstMultiplyingExpressionNode( AstExpressionNode left, AstExpressionNode right )
            : base( AstNodeId.Multiplying, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstMultiplyingExpressionNode()
            : base( AstNodeId.Multiplying )
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
