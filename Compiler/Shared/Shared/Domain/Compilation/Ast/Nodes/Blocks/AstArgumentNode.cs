namespace KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Blocks
{
    /// <summary>
    /// AST node representing an argument
    /// </summary>
    public class AstArgumentNode : AstNode, INameable
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
        public AstArgumentNode() {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstArgumentNode( IAstNode parent )
            : base( AstNodeId.Argument, parent )
        {
        }

        #region IAstNodeAcceptor
        ///
        /// <inheritdoc />
        ///
        public override int ChildNodeCount
            => 0;

        ///
        /// <inheritdoc/>
        ///
        public override IAstNode Accept( IAstVisitor visitor )
        {
            return visitor.Visit( this );
        }

        public override void AcceptChildren( IAstVisitor visitor )
        {
            // Do nothing
        }

        #endregion IAstNodeAcceptor
    }
}
