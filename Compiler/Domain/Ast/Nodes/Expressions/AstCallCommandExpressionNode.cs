namespace KSPCompiler.Domain.Ast.Nodes.Expressions
{
    /// <summary>
    /// AST node representing an invoking the KSP command
    /// </summary>
    public class AstCallCommandExpressionNode : AstExpressionNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstCallCommandExpressionNode( IAstNode parent, AstExpressionNode left, AstExpressionNode right )
            : base( AstNodeId.CallCommandExpression, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstCallCommandExpressionNode( AstExpressionNode left, AstExpressionNode right )
            : base( AstNodeId.CallCommandExpression, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstCallCommandExpressionNode()
            : base( AstNodeId.CallCommandExpression )
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
        public override T Accept<T>( IAstVisitor<T> visitor, AbortTraverseToken abortTraverseToken )
            => visitor.Visit( this , abortTraverseToken );

        #endregion IAstNodeAcceptor
    }
}
