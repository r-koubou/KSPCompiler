namespace KSPCompiler.Domain.Ast.Blocks
{
    /// <summary>
    /// AST node representing an argument
    /// </summary>
    public class AstArgument : AstNode, INameable
    {
        #region INameable

        ///
        /// <inheritdoc/>
        ///
        public string Name { get; set; } = "";

        #endregion INameable

        /// <summary>
        /// Ctor
        /// </summary>
        public AstArgument( IAstNode? parent = null )
            : base( AstNodeId.Argument, parent )
        {
        }

        #region IAstNodeAcceptor

        ///
        /// <inheritdoc/>
        ///
        public override T Accept<T>( IAstVisitor<T> visitor )
            => visitor.Visit( this );

        public override void AcceptChildren<T>( IAstVisitor<T> visitor )
        {
            // Do nothing
        }

        #endregion IAstNodeAcceptor
    }
}
