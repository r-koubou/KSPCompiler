namespace ${namespace}
{
    /// <summary>
    /// AST node representing ${description}
    /// </summary>
    public class ${classname} : AstFunctionalSyntaxNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public ${classname}( IAstNode? parent = null )
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
