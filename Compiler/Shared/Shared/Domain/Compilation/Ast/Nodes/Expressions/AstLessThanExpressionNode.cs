namespace KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions
{
    /// <summary>
    /// AST node representing a comparison operator: less than (&lt;)
    /// </summary>
    public class AstLessThanExpressionNode : AstExpressionNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstLessThanExpressionNode( IAstNode parent, AstExpressionNode left, AstExpressionNode right )
            : base( AstNodeId.LessThan, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstLessThanExpressionNode( AstExpressionNode left, AstExpressionNode right )
            : base( AstNodeId.LessThan, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstLessThanExpressionNode()
            : base( AstNodeId.LessThan )
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
