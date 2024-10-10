namespace ${namespace}
{
    /// <summary>
    /// AST node representing ${description}
    /// </summary>
    public class ${classname} : AstExpressionSyntaxNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public ${classname}( IAstNode parent, AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.${name}, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public ${classname}( AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.${name}, NullAstNode.Instance, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public ${classname}()
            : base( AstNodeId.${name}, NullAstNode.Instance )
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
