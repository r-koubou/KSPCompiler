namespace KSPCompiler.Domain.Ast.Nodes.Expressions
{
    /// <summary>
    /// AST node representing an unary operator: negative
    /// </summary>
    public class AstUnaryMinusExpression : AstExpressionSyntaxNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstUnaryMinusExpression( IAstNode parent, AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.UnaryMinus, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstUnaryMinusExpression( AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.UnaryMinus, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstUnaryMinusExpression()
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
        public override T Accept<T>( IAstVisitor<T> visitor, AbortTraverseToken abortTraverseToken )
            => visitor.Visit( this , abortTraverseToken );

        #endregion IAstNodeAcceptor
    }
}
