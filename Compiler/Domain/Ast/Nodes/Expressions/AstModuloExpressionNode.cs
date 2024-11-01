namespace KSPCompiler.Domain.Ast.Nodes.Expressions
{
    /// <summary>
    /// AST node representing a modulo operator (%)
    /// </summary>
    public class AstModuloExpressionNode : AstExpressionNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstModuloExpressionNode( IAstNode parent, AstExpressionNode left, AstExpressionNode right )
            : base( AstNodeId.Modulo, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstModuloExpressionNode( AstExpressionNode left, AstExpressionNode right )
            : base( AstNodeId.Modulo, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstModuloExpressionNode()
            : base( AstNodeId.Modulo )
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
