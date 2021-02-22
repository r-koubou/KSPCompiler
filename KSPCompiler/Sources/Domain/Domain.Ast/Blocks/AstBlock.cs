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

        /// <summary>
        /// Ctor
        /// </summary>
        public AstBlock( IAstNode parent )
            : base( AstNodeId.Block, parent )
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
            foreach( var n in Statements )
            {
                n.Accept( visitor );
            }
        }
        #endregion IAstNodeAcceptor
    }
}