namespace KSPCompiler.Domain.Ast.Node
{
    /// <summary>
    /// Declaring a constant expressive node.
    /// </summary>
    public interface IImmutable
    {
        /// <summary>
        /// Returns whether the node is a constant expressive node or not.
        /// </summary>
        public bool IsImmutable { get; set; }
    }
}
