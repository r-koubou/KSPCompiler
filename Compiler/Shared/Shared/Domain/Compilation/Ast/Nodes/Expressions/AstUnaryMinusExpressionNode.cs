namespace KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions
{
    /// <summary>
    /// AST node representing an unary operator: negative
    /// </summary>
    public class AstUnaryMinusExpressionNode : AstExpressionNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstUnaryMinusExpressionNode( IAstNode parent, AstExpressionNode left, AstExpressionNode right )
            : base( AstNodeId.UnaryMinus, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstUnaryMinusExpressionNode( AstExpressionNode left, AstExpressionNode right )
            : base( AstNodeId.UnaryMinus, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstUnaryMinusExpressionNode()
            : base( AstNodeId.UnaryMinus )
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
