namespace KSPCompiler.Domain.Ast.Blocks
{
    /// <summary>
    /// AST node representing a callback, block in function
    /// </summary>
    public partial class AstBlock
    {
        /// <summary>
        /// Statement list
        /// </summary>
        public AstNodeList<AstNode> Statements { get; } = new();

        private partial void Initialize()
        {}

        #region Part of IAstNodeAcceptor
        ///
        /// <inheritdoc/>
        ///
        public override void AcceptChildren<T>( IAstVisitor<T> visitor )
        {
            foreach( var n in Statements )
            {
                n.Accept( visitor );
            }
        }
        #endregion IAstNodeAcceptor
    }
}