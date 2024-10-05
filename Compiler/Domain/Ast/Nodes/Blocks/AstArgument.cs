namespace KSPCompiler.Domain.Ast.Nodes.Blocks
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
        public string Name { get; set; } = string.Empty;

        #endregion INameable

        /// <summary>
        /// Ctor
        /// </summary>
        public AstArgument() {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstArgument( IAstNode parent )
            : base( AstNodeId.Argument, parent )
        {
        }

        #region IAstNodeAcceptor

        ///
        /// <inheritdoc/>
        ///
        public override T Accept<T>( IAstVisitor<T> visitor, AbortTraverseToken abortTraverseToken )
        {
            return visitor.Visit( this, abortTraverseToken );
        }

        public override void AcceptChildren<T>( IAstVisitor<T> visitor, AbortTraverseToken abortTraverseToken )
        {
            // Do nothing
        }

        #endregion IAstNodeAcceptor
    }
}
