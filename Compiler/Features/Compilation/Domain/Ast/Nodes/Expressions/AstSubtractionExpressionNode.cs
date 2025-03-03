namespace KSPCompiler.Features.Compilation.Domain.Ast.Nodes.Expressions
{
    /// <summary>
    /// AST node representing a subtraction operator (-)
    /// </summary>
    public class AstSubtractionExpressionNode : AstExpressionNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstSubtractionExpressionNode( IAstNode parent, AstExpressionNode left, AstExpressionNode right )
            : base( AstNodeId.Subtraction, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstSubtractionExpressionNode( AstExpressionNode left, AstExpressionNode right )
            : base( AstNodeId.Subtraction, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstSubtractionExpressionNode()
            : base( AstNodeId.Subtraction )
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
