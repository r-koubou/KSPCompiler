using KSPCompiler.Domain.Ast.Nodes.Blocks;

namespace KSPCompiler.Domain.Ast.Nodes
{
    /// <summary>
    /// The base class of the node corresponding to the function or callback definition.
    /// </summary>
    public abstract class AstFunctionalNode : AstNode, INameable
    {
        /// <summary>
        /// Statements, expressions
        /// </summary>
        public AstBlockNode Block { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        protected AstFunctionalNode(
            AstNodeId id,
            IAstNode parent )
            : base( id, parent )
        {
            Block = new AstBlockNode( this );
        }

        #region INameable
        ///
        /// <inheritdoc/>
        ///
        public string Name { get; set; } = string.Empty;
        #endregion INameable

        #region IAstNodeAcceptor
        ///
        /// <inheritdoc />
        ///
        public override int ChildNodeCount
            => 1;

        ///
        /// <inheritdoc/>
        ///
        public override void AcceptChildren( IAstVisitor visitor )
        {
            Block.AcceptChildren( visitor );
        }
        #endregion IAstNodeAcceptor
    }
}
