namespace KSPCompiler.Domain.Ast.Nodes.Expressions
{
    /// <summary>
    /// AST node representing a bitwise AND operator
    /// </summary>
    public class AstBitwiseAndExpressionNode : AstExpressionNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstBitwiseAndExpressionNode( IAstNode parent, AstExpressionNode left, AstExpressionNode right )
            : base( AstNodeId.BitwiseAnd, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstBitwiseAndExpressionNode( AstExpressionNode left, AstExpressionNode right )
            : base( AstNodeId.BitwiseAnd, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstBitwiseAndExpressionNode()
            : base( AstNodeId.BitwiseAnd )
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
