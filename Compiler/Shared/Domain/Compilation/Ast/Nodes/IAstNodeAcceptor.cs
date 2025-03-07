namespace KSPCompiler.Shared.Domain.Compilation.Ast.Nodes
{
    /// <summary>
    /// Define an Acceptor that accepts <see cref="IAstVisitor{T}"/>.
    /// </summary>
    public interface IAstNodeAcceptor
    {
        /// <summary>
        /// Get the number of child nodes.
        /// </summary>
        public int ChildNodeCount { get; }

        /// <summary>
        /// Acceptance process for visitors.
        /// </summary>
        public IAstNode Accept( IAstVisitor visitor );

        /// <summary>
        /// Acceptance process of a visitor to a contained node such as a child node.
        /// </summary>
        public void AcceptChildren( IAstVisitor visitor );
    }
}
