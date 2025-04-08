namespace ${namespace}
{
    /// <summary>
    /// AST node representing ${description}
    /// </summary>
    public class ${classname}Node : AstExpressionNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public ${classname}Node( IAstNode parent, AstExpressionNode left, AstExpressionNode right )
            : base( AstNodeId.${name}, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public ${classname}Node( AstExpressionNode left, AstExpressionNode right )
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
        public override IAstNode Accept( IAstVisitor visitor )
            => visitor.Visit( this );

        #endregion IAstNodeAcceptor
    }
}
