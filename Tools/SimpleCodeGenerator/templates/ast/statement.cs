namespace ${namespace}
{
    /// <summary>
    /// AST node representing ${description}
    /// </summary>
    public class ${classname}Node : AstStatementNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public ${classname}Node() : this( NullAstNode.Instance ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public ${classname}Node( IAstNode parent )
            : base( AstNodeId.${name}, parent )
        {
        }

        #region IAstNodeAcceptor

        ///
        /// <inheritdoc/>
        ///
        public override T Accept<T>( IAstVisitor<T> visitor )
            => visitor.Visit( this );

        ///
        /// <inheritdoc/>
        ///
        public override void AcceptChildren<T>( IAstVisitor<T> visitor )
        {
            throw new System.NotImplementedException();
        }

        #endregion IAstNodeAcceptor
    }
}
