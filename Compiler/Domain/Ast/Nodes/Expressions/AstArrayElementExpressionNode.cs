namespace KSPCompiler.Domain.Ast.Nodes.Expressions
{
    /// <summary>
    /// AST node representing an array element reference expression ( Left: AstDefaultExpressionNode, Right: AstExpressionNode )
    /// </summary>
    public class AstArrayElementExpressionNode : AstExpressionNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstArrayElementExpressionNode( IAstNode parent )
            : this( parent, NullAstExpressionNode.Instance, NullAstExpressionNode.Instance ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstArrayElementExpressionNode( IAstNode parent, AstExpressionNode left, AstExpressionNode right )
            : base( AstNodeId.ArrayElementExpression, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstArrayElementExpressionNode( AstExpressionNode left, AstExpressionNode right )
            : base( AstNodeId.ArrayElementExpression, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstArrayElementExpressionNode()
            : base( AstNodeId.ArrayElementExpression )
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
