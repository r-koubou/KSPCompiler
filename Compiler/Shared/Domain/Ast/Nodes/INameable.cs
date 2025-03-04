namespace KSPCompiler.Shared.Domain.Ast.Nodes
{
    /// <summary>
    /// Declaration of Individually Named Nodes.
    /// </summary>
    public interface INameable
    {
        /// <summary>
        /// Any name that identifies the node.
        /// </summary>
        public string Name { get; set; }
    }
}
