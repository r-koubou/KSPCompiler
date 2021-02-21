__using__

namespace __namespace__
{
    /// <summary>
    /// AST node representing __desc__
    /// </summary>
    public partial class __name__ : __base__
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public __name__( IAstNode parent )
            : base( AstNodeId.__ast_id__, parent )
        {
            Initialize();
        }

        private partial void Initialize();

        #region Part of IAstNodeAcceptor
        ///
        /// <inheritdoc/>
        ///
        public override T Accept<T>( IAstVisitor<T> visitor )
        {
            return visitor.Visit( this );
        }
        #endregion IAstNodeAcceptor
    }
}