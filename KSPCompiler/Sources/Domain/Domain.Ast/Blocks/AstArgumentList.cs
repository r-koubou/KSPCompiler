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

        private partial void Initialize()
        {}

        #region Part of IAstNodeAcceptor
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