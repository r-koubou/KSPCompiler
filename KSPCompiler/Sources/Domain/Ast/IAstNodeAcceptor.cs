using KSPCompiler.Domain.Ast.Node;

namespace KSPCompiler.Domain.Ast
{
    /// <summary>
    /// Define an Acceptor that accepts <see cref="IAstVisitor{T}"/>.
    /// </summary>
    public interface IAstNodeAcceptor
    {
        /// <summary>
        /// Acceptance process for visitors.
        /// </summary>
        public T Accept<T>( IAstVisitor<T> visitor );

        /// <summary>
        /// Acceptance process of a visitor to a contained node such as a child node.
        /// </summary>
        public void AcceptChildren<T>( IAstVisitor<T> visitor );
    }
}
