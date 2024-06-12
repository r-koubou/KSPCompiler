namespace KSPCompiler.Domain.Ast.Node
{
    /// <summary>
    /// Null Object of <see cref="AstNode"/>
    /// </summary>
    public class NullAstNode : AstNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public NullAstNode( IAstNode parent ) : base( AstNodeId.None, parent ) {}

        #region IAstNodeAcceptor
        ///
        /// <inheritdoc/>
        ///
        public override T Accept<T>( IAstVisitor<T> visitor )
            => visitor.Visit( this );

        ///
        /// <inheritdoc/>
        ///
        public override void AcceptChildren<T>( IAstVisitor<T> visitor ) {}
        #endregion IAstNodeAcceptor
    }
}
