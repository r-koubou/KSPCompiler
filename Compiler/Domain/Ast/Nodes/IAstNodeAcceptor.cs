namespace KSPCompiler.Domain.Ast.Nodes
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
        public T Accept<T>( IAstVisitor<T> visitor, AbortTraverseToken abortTraverseToken );

        /// <summary>
        /// Acceptance process of a visitor to a contained node such as a child node.
        /// </summary>
        public void AcceptChildren<T>( IAstVisitor<T> visitor, AbortTraverseToken abortTraverseToken );
    }
}
