namespace ${namespace}
{
    /// <summary>
    /// AST node representing ${description}
    /// </summary>
    public class ${classname}Node : AstFunctionalNode
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


        #endregion IAstNodeAcceptor
    }
}
