namespace KSPCompiler.Domain.Ast.Blocks
{
    /// <summary>
    /// AST node representing an argument list
    /// </summary>
    public partial class AstArgumentList
    {
        /// <summary>
        /// Argument node list
        /// </summary>
        public AstNodeList<AstArgument> Arguments { get; } = new();

        public bool HasArgument => Arguments.Count > 0;
        public int ArgumentCount => Arguments.Count;

        /// <summary>
        /// Ctor
        /// </summary>
        public AstArgumentList( IAstNode parent )
            : base( AstNodeId.ArgumentList, parent )
        {}

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
            // Do nothing
        }
        #endregion IAstNodeAcceptor
    }
}