namespace KSPCompiler.Domain.Ast.Nodes.Expressions
{
    /// <summary>
    /// AST node representing a division operator (/)
    /// </summary>
    public class AstDivisionExpressionNode : AstExpressionNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstDivisionExpressionNode( IAstNode parent, AstExpressionNode left, AstExpressionNode right )
            : base( AstNodeId.Division, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstDivisionExpressionNode( AstExpressionNode left, AstExpressionNode right )
            : base( AstNodeId.Division, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstDivisionExpressionNode()
            : base( AstNodeId.Division )
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
