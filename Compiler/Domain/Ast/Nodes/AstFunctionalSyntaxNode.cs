using KSPCompiler.Domain.Ast.Nodes.Blocks;

namespace KSPCompiler.Domain.Ast.Nodes
{
    /// <summary>
    /// The base class of the node corresponding to the function or callback definition.
    /// </summary>
    public abstract class AstFunctionalSyntaxNode : AstNode, INameable
    {
        /// <summary>
        /// Statements, expressions
        /// </summary>
        public AstBlock Block { get; set; }

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
        #endregion INameable

        #region IAstNodeAcceptor
        ///
        /// <inheritdoc/>
        ///
        public override void AcceptChildren<T>( IAstVisitor<T> visitor, AbortTraverseToken abortTraverseToken )
        {
            Block.AcceptChildren( visitor, abortTraverseToken );
        }
        #endregion IAstNodeAcceptor
    }
}
