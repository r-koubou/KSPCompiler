namespace KSPCompiler.Domain.Ast.Nodes.Expressions
{
    /// <summary>
    /// AST node representing a conditional operator: XOR (xor)
    /// </summary>
    public class AstLogicalXorExpression : AstExpressionSyntaxNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstLogicalXorExpression( IAstNode parent, AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.LogicalXor, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstLogicalXorExpression( AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.LogicalXor, NullAstNode.Instance, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstLogicalXorExpression()
            : base( AstNodeId.LogicalXor, NullAstNode.Instance )
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
            => visitor.Visit( this, abortTraverseToken );

        #endregion IAstNodeAcceptor
    }
}
