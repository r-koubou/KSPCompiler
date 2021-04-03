namespace KSPCompiler.Domain.Ast.Blocks
{
    /// <summary>
    /// The base class of the node corresponding to the function or callback definition.
    /// </summary>
    public abstract class AstFunctionalSyntaxNode : AstNode, INameable
    {
        /// <summary>
        /// Statements, expressions
        /// </summary>
        public AstBlock Block { get; protected set; }

        /// <summary>
        /// Ctor
        /// </summary>
        protected AstFunctionalSyntaxNode(
            AstNodeId id,
            IAstNode parent )
            : base( id, parent )
        {
            Block = new AstBlock( this );
        }

        #region INameable
        ///
        /// <inheritdoc/>
        ///
        public string Name { get; set; } = string.Empty;
        #endregion IAstNameable

        #region IAstNodeAcceptor
        ///
        /// <inheritdoc/>
        ///
        public override T Accept<T>( IAstVisitor<T> visitor )
        {
            return visitor.Visit( this );
        }

        ///
        /// <inheritdoc/>
        ///
        public override void AcceptChildren<T>( IAstVisitor<T> visitor )
        {
            Block.AcceptChildren( visitor );
        }
        #endregion IAstNodeAcceptor
    }
}
